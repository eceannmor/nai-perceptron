# Perceptron - Single-neuron network

This project was made as a part of the NAI (Narzędzia Sztucznej Inteligencji / Artificial Intelligence Tools) course at [PJATK](https://pja.edu.pl)

This is a simple console application that provides the GUI for viewing the results of the training process, with the added ability to test the perceptron in realtime.

Upon launch, you will be prompted with a menu:
Use the `Up` and `Down` arrow keys to navigate the menu, press `Enter` to access the selected option 
```c
> Test custom weights
Test the test set
Exit
```
The menu of the custom weights tab works similarly. Use `Enter` to enter and exit edit mode when focused on one of the weights.  
When in edit mode, you can use your keyboard to enter new weights.  
Input is limited to numbers, decimal point and `Backspace`
```c
> w1 = 0.0
w2 = 0.0
w3 = 0.0
w4 = 0.0
Exit

Current prediction: Iris-versicolor
```
`Current prediction` updates live as you type the weights in