#include<iostream>
#include <queue>
#include <vector>
#include <algorithm>
#include <iterator>
#include <string>
#define arrLen 10
using namespace std;

struct heapNode {
	char sym;
	int val;
	heapNode* left_child, * right_child;
	heapNode(char sym, int val) {
		left_child = right_child = NULL;
		this->sym = sym;
		this->val = val;
	}
};

void deleteTree(heapNode* node) {
	if (node == nullptr) return;
	deleteTree(node->left_child);
	deleteTree(node->right_child);
	delete node;
}

void codesToArr(struct heapNode* root, char (&symb)[arrLen], string (&freq)[arrLen], string str, int& i) {
	if (!root) {
		return;
	}
	if (root->sym != '+' && i != arrLen) {
		symb[i] = root->sym;
		freq[i] = str;
		i++;
	}

	codesToArr(root->left_child, symb, freq, (str + "0"), i);
	codesToArr(root->right_child, symb, freq, (str + "1"), i);
}

void huffCodes(char s[], int v[], char (&chars)[arrLen], string (&keys)[arrLen], int len) {
	
	struct heapNode* left_child, * right_child, * root;
	auto sortNode = [](const heapNode* a, const heapNode* b) {
		return a->val > b->val;
	};
	priority_queue<heapNode*, vector<heapNode*>, decltype(sortNode) > minheap(sortNode);
	for (int i = 0; i < len; i++) {
		minheap.push(new heapNode(s[i], v[i]));
	}
	while (minheap.size() != 1) {
		left_child = minheap.top();
		minheap.pop();

		right_child = minheap.top();
		minheap.pop();

		heapNode* tmp = new heapNode('+', (left_child->val + right_child->val));
		tmp->left_child = left_child;
		tmp->right_child = right_child;

		minheap.push(tmp);
	}
	int i = 0;
	codesToArr(minheap.top(), chars, keys, "", i);
	deleteTree(minheap.top()); 
}

string hcAlg(string main, string keys[], char chars[]) {
	char tmp;
	int index = 0;
	string res = "";
	for (int i = 0; i < arrLen; i++) {
		tmp = main[i];
		char* ptr = find(chars, chars + arrLen, tmp);
		if (ptr != chars + arrLen) {
			index = distance(chars, ptr);
		}
		res += keys[index];
	}
	return res;
}
string rev_hcAlg(string res, string keys[], char chars[]) {
	string main = "";
	string tmp = "";
	int ind;
	int i = 0;
	int k = 0;
	while (i!= res.size()) {
		tmp += res[i];
		k = 0;
		for (ind = 0; ind < arrLen; ++ind) {
			if (keys[ind] == tmp) {
				main += chars[ind];
				k++;
				tmp = "";
				i++;
				break;
			}
		}
		if (k == 0) {
			i++;
		}
	}
	return main;
}
int main() {
	setlocale(LC_ALL, "Russian");
	int len = arrLen;
	string keys[arrLen];
	char chars[arrLen];
	char symbols [] = {'Л', 'е', 'в', ' ', 'Т','о', 'л', 'с', 'т', 'й'};
	int vals[] = { 1, 1, 1, 1, 1, 2, 1, 1, 2, 1 };
	cout << "Initial conditions" << endl;
	for (int j = 0; j < arrLen; j++) {
		cout << symbols[j] << ": " << vals[j] << endl;
	}
	huffCodes(symbols, vals, chars, keys, len);
	cout << "Huffman codes" << endl;
	for (int i = 0; i < arrLen; i++) {
		cout << chars[i] << ": " << keys[i] << endl;
	}
	string main = "Лев Толстой";
	string res = hcAlg(main, keys, chars);
	cout << "Direct conversion of: " << main << " is " << res << endl;
	/*main = rev_hcAlg(res, keys, chars);
	cout << "Reverse conversion: " << main << endl;*/
	return 0;
}