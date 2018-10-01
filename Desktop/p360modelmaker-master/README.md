# BASIC CODE TO RUN THE MODEL CLASSIFICATION

## Pre Requisites. 

- You need a python interpreter. I use Anaconda, if you download anaconda, it comes built in with a lot of libraries I already use like matplotlib 
- I'm using python 3.6.4
	- You might have to specify your python path to get pip running fine. If you already have python 2.7 installed and you download python 3, to use python 3 you'll have to run *python3* instead of *python* and *pip3* instead of *pip*
- You need to install tensorflow version 1.8
	- familiarize yourself with tensorflow
- you need to install pillow
	- run the command 
		- pip install pillow
- you need to install opencv
	- this is a bit trickier, try running the command below but I've read some stack overflow issues where installing this binary might not have all the video processing features we need
		- pip install opencv


## Hopefully I remember all the libraries I installed and used

- To start the code, which uses your video camera, run
	- python main.py 

- To quit the code press q

- Before we get started with the docker portion, I need you to get your feet wet with the basics of tensorflow.

- Look up some of the slides here 
	- http://web.stanford.edu/class/cs20si/syllabus.html

- I need you to have an idea of how I'm running the recognition code so here's the link of the API which I've been using.
	- https://github.com/tensorflow/models/tree/master/research/object_detection

- I need you to catch up to speed with how the core functions work before we start creating the django server and run the classification model on the server.
	- And thanks again Ronit for working with me on this 	