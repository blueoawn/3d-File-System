# -*- coding: utf-8 -*-
"""
Created on Tue Apr 10 11:34:14 2018

@author: Darren Liu
"""

#--xml_path=path_to_xml_folder


import math
import globs
import numpy as np
import pandas as pd
import tensorflow as tf
import xml.etree.ElementTree as ET


flags = tf.app.flags
flags.DEFINE_string('xml_path', '', 'Path to xml files')
FLAGS = flags.FLAGS


def xml_to_csv(path):
    xml_list = []
    for xml_file in glob.glob(path + '/*.xml'):
        tree = ET.parse(xml_file)
        root = tree.getroot()
        for member in root.findall('object'):
            value = (root.find('filename').text,
                     int(root.find('size')[0].text),
                     int(root.find('size')[1].text),
                     member[0].text,
                     int(member[4][0].text),
                     int(member[4][1].text),
                     int(member[4][2].text),
                     int(member[4][3].text)
                     )
            xml_list.append(value)
    column_name = ['filename', 'width', 'height', 'class', 'xmin', 'ymin', 'xmax', 'ymax']
    xml_df = pd.DataFrame(xml_list, columns=column_name)
    return xml_df


def split_labels(xml_df,training_percent):
    
    np.random.seed(1)
    grouped = xml_df.groupby('filename')
    grouped_list = [grouped.get_group(x) for x in grouped.groups]
    
    training_size = math.floor(len(grouped_list) /(training_percent/100))
    
    train_index = np.random.choice(len(grouped_list), size=training_size)
    test_index = np.setdiff1d(list(range(len(grouped_list))), train_index)
    
    train = pd.concat([grouped_list[i] for i in train_index])
    test = pd.concat([grouped_list[i] for i in test_index])
    
    train.to_csv('train_labels.csv', index=None)
    test.to_csv('test_labels.csv', index=None)
    
    


def main():
    xml_df = xml_to_csv(FLAGS.xml_path)
    split_labels(xml_df,80)



if __name__ == '__main__':
    main()
#334 test files