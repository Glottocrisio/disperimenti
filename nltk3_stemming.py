# -*- coding: utf-8 -*-
"""
Created on Sat Mar  2 18:53:33 2019
Lesson 3  stemming
@author: Cosimo
"""

from nltk.stem import PorterStemmer

from nltk.tokenize import word_tokenize

ps= PorterStemmer()

ex=["python","pythoner","pythoning","pythoned","pythonly"]

for w in ex:
    print(ps.stem(w))