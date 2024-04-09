#include <cmath>
#include <vector>
#include <iostream>
#include <cstdlib>
#include <ctime>
using namespace std;
double calculateEntropy(double (&probabilities)[5]) {
    double entropy = 0.0;
    for (int i = 0; i < 5; i++) {
        if (probabilities[i] > 0) {
            entropy -= probabilities[i] * log2(probabilities[i]);
        }
    }
    return entropy;
}

int main() {
    double probabilities[5] = { 0.1, 0.2, 0.3, 0.3, 0.1 };
    double result;
    result = calculateEntropy(probabilities);
    cout << result << endl;
    return 0;
}