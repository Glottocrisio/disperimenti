# -*- coding: utf-8 -*-
"""
Created on Wed Jul 17 16:22:12 2019

@author: Cosimo

chinking- removal of something (what to keep out of the chunk)
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
            chunkGram= r"""Chunk: {<.*>+?}
                                    }<VB.? | IN| DT>+{"""
            chunkParser= nltk.RegexpParser(chunkGram)
            chunked=chunkParser.parse(tagged)
            chunked.draw()
           
    except Exception as e:
        print (str(e))
         
process_content()