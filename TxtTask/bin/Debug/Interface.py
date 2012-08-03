#!/usr/bin/python

import ScyTasksInterface as interface
import ScyTasks
import sys

validChars = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', ':', ';', '\"', "'", '[', '{', ']', '}', '|', '`', '~', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', ',', '<', '.', '>', '?', ' ']

def removeTask(task):
    if type(task) == str:
        if (hasEscapes(task)):
            task = removeEscapes(task)
        try:
            interface.deleteTask(interface.getTaskID(task))
        except Exception as e:
            print e.content

def createTask(task):
    if type(task) == str:
        if (hasEscapes(task)):
            task = removeEscapes(task)
        try:
            interface.createTask(task, None, None)
        except Exception as e:
            print e.content

def refresh():
    ScyTasks.main()

def hasEscapes(string):
    n = 0;
    for i in range(len(string)):
        if not contains(validChars, string[i]):
            n += 1
    if n > 0: return True
    else: return False

def removeEscapes(string):
    retStr = ''
    for i in range(len(string)):
        if contains(validChars, string[i]): retStr += string[i]
    return retStr

def contains(ls, char):
    if char in ls:
        return True
    for i in ls:
        if i.swapcase() == char:
            return True
    return False
    
if __name__ == '__main__':
    if '-remove' in sys.argv: removeTask(sys.argv[sys.argv.index('-remove')+1])
    elif '-create' in sys.argv: createTask(sys.argv[sys.argv.index('-create')+1])
    elif '-refresh' in sys.argv: refresh()
