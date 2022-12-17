#include "pch.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <string>


using std::string;
using std::cin;
using std::endl;
using std::cout;
using std::cerr;
using std::string;
using std::ifstream;
using std::ostringstream;

// link Header file
#include "tokenizer.h"

using namespace std;

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

string parse(string og) {
	std::cout << "TOKENIZER RUNNING!" << std::endl;

	return "meow";
}