# -*- coding: utf-8 -*-
"""
Created on Sun Oct 28 21:35:38 2018

@author: Cosimo

NLP Lesson 1- tokenize

"""

from nltk.tokenize import sent_tokenize, word_tokenize


ex= "hello df fdfdfd vgdf d. fdf.dd  dfdfdfd. dffdfr f e be  ef?"
print(sent_tokenize(ex))
print(word_tokenize(ex))
print ()

for i in (word_tokenize(ex)):
    print(i)
    