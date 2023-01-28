#include "pch.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <string>

// Catoscript custom imports lol

#include "tokenizer.h"

using std::cin;
using std::endl;
using std::cout; 
using std::cerr; 
using std::string; 
using std::ifstream; 
using std::ostringstream;

using namespace std;

string readFileIntoString(const string& path) {
	auto ss = ostringstream{};
	ifstream input_file(path);
	if (!input_file.is_open()) {
		cerr << "Could not open the file - '" << path << "'" << endl;
		return "0";
	}
	else {
		ss << input_file.rdbuf();
		return ss.str();
	}
}

vector<string> explode(const string& str, const char& ch) {
	string next;
	vector<string> result;

	// For each character in the string
	for (string::const_iterator it = str.begin(); it != str.end(); it++) {
		// If we've hit the terminal character
		if (*it == ch) {
			// If we have some characters accumulated
			if (!next.empty()) {
				// Add them to the result vector
				result.push_back(next);
				next.clear();
			}
		}
		else {
			// Accumulate the next character into the sequence
			next += *it;
		}
	}
	if (!next.empty())
		result.push_back(next);
	return result;
}

int main(int argc,char* argv[])
{
	std::string filename = "";
	if (argc > 1) {
		std::cout << argv[1] << endl;
		filename = argv[1];
	}
	else {
		filename = "script/test.cato";
	}
	string file_contents;      
	file_contents = readFileIntoString(filename);
	std::vector<std::string> result = explode(file_contents, ';');
	if (file_contents == "0") {
		exit(404);
	}
	string parsed = "";
	for (size_t i = 0; i < result.size(); i++) {
		cout << "\"" << result[i] << "\"" << endl;
		parsed = parsed + parse(result[i]);
	}

	// cout << file_contents << endl;
	
	cout << parsed << endl;
	cout << "Press enter to continue!";
	cin.get();
	return 0;
}
