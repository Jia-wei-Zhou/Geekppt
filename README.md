# Geekppt

## A powerful slide making tool -- execute code in textbox

This is a MSc Computing group project in Imperial College London -- "A literate programming environment for PowerPoint". 

Prominent literate programming environments nowadays are Jupyter notebooks, Mathematica, emacs' org-mode, etc. While all of them can produce slides, none of them supports the generation of PowerPoint presentations. 

In this project, we aim to build a ppt addin called ```Code Evaluation``` that helps quickly insert code output, such as print and pictures in ppt, and enable command line and table input. This addin also supports shortcut conversion from multiple types to pptx while preserving all fhe features above.

## Dependencies

TBI by 3 giants

## Installation

TBI 3 giants

## User Guide

TBI by HH, ZQH, WZF
### Prerequisites

TBI by WZF

### Create codebox, click to execute

Click on a language button to create a codebox. For example, to create a Python codebox, simply click on the ```Python``` button, type in some code (such as ```print("Hello")```). To execute the code, click the ```evaluate``` button, "Hello" will be shown in a newly inserted textbox. 

Currently the addin supports C++, Java and Python. It is worth noting that using C++ and Java requires creating main function and helper functions with different buttons. Before executing main function with multiple helper functions, make sure they are the same language, then choose them all and click ```evaluate``` to get results. 

### add input to you code

Adding inputs can be easy using the ```General Inputs``` button. After creating a input box, choose it with codeboxes that requires input, click ```evaluate``` to get results. 

### graphical output

Graphical output can be triggered using ```plt.show()``` in Python. A ```plt.show()``` will result in a figure inserted in current slide. You can use multiple ```plt.show()``` to insert many images. The addin does not support ```plt.show()``` in loops.

Copy the following Python code in a codebox and try ```evaluate``` it to check the effect.

```
import numpy as np
import matplotlib.pyplot as plt

x = np.linspace(0, 5, 100)  # Sample data.
fig, ax = plt.subplots(figsize=(6, 2.7))

ax.plot(x, x, label='linear')
ax.plot(x, x**2, label='quadratic')
ax.plot(x, x**5, label='pow5') 
ax.set_xlabel('x label')
ax.set_ylabel('y label')
ax.set_title("Simple Plot")
ax.legend()
plt.show()
```

### parameter table and plot template

Use ```Parameter Table``` to generate a parameter table where you can add any row or column and write data into cells. The first row of the table defaults to the table name, which means that it will not be entered as an argument. It would help if you replaced the table name with the appropriate place in the code block to ensure it doesn't go wrong.

You can then select a parameter table and click the ```Plot Template button``` to generate a block of Python code. Using it, you can draw a simple scatter plot (based on the data in the table).

### conversion to ppt with codebox enabled

TBI by WZF

## Technical Documentation

TBI by LWX, ZJW

## License

TBI by ZQH
