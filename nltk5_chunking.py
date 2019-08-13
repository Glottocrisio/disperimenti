# -*- coding: utf-8 -*-
"""
Created on Tue Jul 16 11:30:44 2019

@author: Cosimo

chunking
"""

import nltk

from nltk.corpus import state_union
from nltk.tokenize import PunktSentenceTokenizer

train= state_union.raw("2005-GWBush.txt")
sample= state_union.raw("2006-GWBush.txt")

custom_sent_tok= PunktSentenceTokenizer(train)
tokenized=custom_sent_tok.tokenize(sample)

def process_content():
    try:
        for i in tokenized:
            words= nltk.word_tokenize(i)
            tagged= nltk.pos_tag(words)
            chunkGram= r"""Chunk: {<RB.?>*<NNP><NN>?} """
            chunkParser= nltk.RegexpParser(chunkGram)
            chunked=chunkParser.parse(tagged)
            
            print(chunked)
            
     except Exception as e:
        print (str(e))
         
process_content()