# -*- coding: utf-8 -*-
"""
Created on Sun Mar 10 22:04:49 2019

NLP part 4 
@author: Cosimo

            
"""

import nltk
from nltk.corpus import state_union
from nltk.tokenize import PunktSentenceTokenizer

sample= state_union.raw("2006-GWBush.txt")
train= state_union.raw("2005-GWBush.txt")
custom_sent_tok= PunktSentenceTokenizer(train)
tokenized=custom_sent_tok.tokenize(sample)

def process_content():
    try:
        for i in tokenized:
            words= nltk.word_tokenize(i)
            tagged= nltk.pos_tag(words)
            
            print(tagged)
         
    except Exception as ex:
        print(str(ex))
        
process_content()