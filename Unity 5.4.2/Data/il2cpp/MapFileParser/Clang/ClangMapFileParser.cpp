#include "ClangMapFileParser.h"
#include "ClangSectionParser.h"
#include "ClangSymbolParser.h"
#include <sstream>
#include <algorithm>
#include "../ParseBuffer.h"

namespace mapfileparser
{

static std::string ReadUntilLine (std::istream& is, const char* delimiterLine)
{
	std::string line;
	std::getline (is, line);
	while (!is.eof () && line != delimiterLine)
		std::getline (is, line);

	return line;
}

static void ReadObjectFiles(std::istream& is, std::vector<std::string>& objectFiles)
{
	std::string line;
	const char* symbolsHeader = "# Sections:";

	std::getline(is, line);
	while (!is.eof() && line != symbolsHeader)
	{
		int32_t objectFileIndex = 0;
		size_t lineLength = line.length();
		ParseBuffer name(lineLength);

		sscanf(line.c_str(), "[%d] %s", &objectFileIndex, name.buffer);
		objectFiles.push_back(std::string(name.buffer));

		std::getline(is, line);
	}
}

static void ReadSections (std::istream& is, std::vector<Section>& sections)
{
	std::string line;
	const char* symbolsHeader = "# Symbols:";

	std::getline (is, line);
	while (!is.eof () && line != symbolsHeader)
	{
		sections.push_back (ClangSectionParser::Parse (line));

		std::getline (is, line);
	}
}

static void ReadSymbols (std::istream& is, std::vector<Symbol>& symbols, const std::vector<std::string>& objectFiles)
{
	std::string line;
	int32_t previousObjectFileIndex = 0;
	while (!is.eof ())
	{
		std::getline (is, line);
		if (line.length () == 0)
			continue;
		ClangSymbol clangSymbol = ClangSymbolParser::Parse (line);
		clangSymbol.symbol.segmentType = kSegmentTypeCode;
		clangSymbol.symbol.objectFile = objectFiles[clangSymbol.objectFileIndex];

		// We stop processing when the index restarts, since this indicates non-function symbols.
		// This also stops us from processing symbols that are not in the code segment, so we can
		// assume this symbol is not data.
		if (clangSymbol.objectFileIndex < previousObjectFileIndex)
			break;
		else
			previousObjectFileIndex = clangSymbol.objectFileIndex;

		symbols.push_back (clangSymbol.symbol);
	}
}

MapFile ClangMapFileParser::Parse (std::istream& is)
{
	MapFile mapFile;
	std::string line;

	ReadUntilLine(is, "# Object files:");
	std::vector<std::string> objectFiles;
	ReadObjectFiles(is, objectFiles);

	// Read sections headers line
	std::getline (is, line);

	ReadSections (is, mapFile.sections);

	// Read symbols headers line
	std::getline (is, line);

	ReadSymbols (is, mapFile.symbols, objectFiles);

	return mapFile;
}

}
