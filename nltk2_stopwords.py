# -*- coding: utf-8 -*-
"""
Created on Sat Mar  2 18:59:39 2019
Lesson 2- Stop Words

@author: Cosimo
"""


from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize


ex="A belief in God is deeply embedded in the human brain, which is programmed for religious experiences, according to a study that analyses why religion is a universal human feature that has encompassed all cultures throughout history."

stopw= set(stopwords.words("english"))
 
words=word_tokenize(ex)
#
#filtersent=[]
#
#for w in words:
#    if w not in stopw:
#        filtersent.append(w)
        
#        
#
#
#print (filtersent)


filtersent= [w for w in words if not w in stopw]
print(filtersent)