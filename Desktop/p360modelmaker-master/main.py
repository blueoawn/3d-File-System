# main python script to view video feed with object detector
import notifications_storage as ns
import numpy as np
import os
import six.moves.urllib as urllib
import sys
import tarfile
import tensorflow as tf
import notifications as n
import zipfile
import time
import boto3
import storeFile as sf






from collections import defaultdict
from io import StringIO
from matplotlib import pyplot as plt

from PIL import Image


# This is needed since the notebook is stored in the object_detection folder.
sys.path.append("..")
from object_detection.utils import ops as utils_ops

#%matplotlib inline

from object_detection.utils import label_map_util

from object_detection.utils import visualization_utils as vis_util



# Path to frozen detection graph. This is the actual model that is used for the object detection.
#PATH_TO_CKPT = MODEL_NAME + '/frozen_inference_graph.pb'
PATH_TO_CKPT = './models/finished/frozen_inference_graph.pb'

# List of the strings that is used to add correct label for each box.
PATH_TO_LABELS = os.path.join('./data', 'object-detection.pbtxt')

NUM_CLASSES = 2

print(tf.__version__)



################?
detection_graph = tf.Graph()
with detection_graph.as_default():
    od_graph_def = tf.GraphDef()
    with tf.gfile.GFile(PATH_TO_CKPT, 'rb') as fid:
        serialized_graph = fid.read()
        od_graph_def.ParseFromString(serialized_graph)
        tf.import_graph_def(od_graph_def, name='')



#####
label_map = label_map_util.load_labelmap(PATH_TO_LABELS)
categories = label_map_util.convert_label_map_to_categories(label_map, max_num_classes=NUM_CLASSES, use_display_name=True)
category_index = label_map_util.create_category_index(categories)




###################
def load_image_into_numpy_array(image):
    (im_width, im_height) = image.size
    return np.array(image.getdata()).reshape((im_height, im_width, 3)).astype(np.uint8)


def save_video_in_rest_call(image):

    save_video_in_rest_call(image);



def video_capture():
    seconds = 0
    cap_video = cv2.VideoCapture(0)
    # Define the codec and create VideoWriter object
    fourcc = cv2.VideoWriter_fourcc(*'XVID')
    output = cv2.VideoWriter('output2.avi',fourcc, 20.0, (640,480))
    while(cap_video.isOpened()):
        ret, frame = cap_video.read()
        if ret==True:
            frame = cv2.flip(frame,0)
            # write the flipped frame
            print("in video capture")
            output.write(frame)
            cv2.imshow('frame',frame)
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
        else:
            break

    cap_video.release()
    output.release()
    print("released")
    cv2.destroyAllWindows()





###############
def run_inference_for_single_image(image, graph):
    with graph.as_default():
        with tf.Session() as sess:
            # Get handles to input and output tensors
            ops = tf.get_default_graph().get_operations()
            all_tensor_names = {output.name for op in ops for output in op.outputs}
            tensor_dict = {}
            for key in ['num_detections', 'detection_boxes', 'detection_scores','detection_classes', 'detection_masks']:
                tensor_name = key + ':0'
                if tensor_name in all_tensor_names:
                    tensor_dict[key] = tf.get_default_graph().get_tensor_by_name(tensor_name)
            if 'detection_masks' in tensor_dict:
                # The following processing is only for single image
                detection_boxes = tf.squeeze(tensor_dict['detection_boxes'], [0])
                detection_masks = tf.squeeze(tensor_dict['detection_masks'], [0])
                # Reframe is required to translate mask from box coordinates to image coordinates and fit the image size.
                real_num_detection = tf.cast(tensor_dict['num_detections'][0], tf.int32)
                detection_boxes = tf.slice(detection_boxes, [0, 0], [real_num_detection, -1])
                detection_masks = tf.slice(detection_masks, [0, 0, 0], [real_num_detection, -1, -1])
                detection_masks_reframed = utils_ops.reframe_box_masks_to_image_masks(detection_masks, detection_boxes, image.shape[0], image.shape[1])
                detection_masks_reframed = tf.cast(tf.greater(detection_masks_reframed, 0.5), tf.uint8)
                # Follow the convention by adding back the batch dimension
                tensor_dict['detection_masks'] = tf.expand_dims(detection_masks_reframed, 0)
            image_tensor = tf.get_default_graph().get_tensor_by_name('image_tensor:0')
            # Run inference
            output_dict = sess.run(tensor_dict,feed_dict={image_tensor: np.expand_dims(image, 0)})
            # all outputs are float32 numpy arrays, so convert types as appropriate
            output_dict['num_detections'] = int(output_dict['num_detections'][0])
            output_dict['detection_classes'] = output_dict['detection_classes'][0].astype(np.uint8)
            output_dict['detection_boxes'] = output_dict['detection_boxes'][0]
            output_dict['detection_scores'] = output_dict['detection_scores'][0]
            if 'detection_masks' in output_dict:
                output_dict['detection_masks'] = output_dict['detection_masks'][0]
    return output_dict



#############
# For the sake of simplicity we will use only 2 images:
# image1.jpg
# image2.jpg
# If you want to test the code with your images, just add path to the images to the TEST_IMAGE_PATHS.
PATH_TO_TEST_IMAGES_DIR = 'images_to_run'
TEST_IMAGE_PATHS = [ os.path.join(PATH_TO_TEST_IMAGES_DIR, 'images{}.jpg'.format(i)) for i in range(1, 6) ]

# Size, in inches, of the output images.
IMAGE_SIZE = (12, 8)


##########################
# for image_path in TEST_IMAGE_PATHS:
# 	image = Image.open(image_path)
# 	# the array based representation of the image will be used later in order to prepare the
# 	# result image with boxes and labels on it.
# 	image_np = load_image_into_numpy_array(image)
# 	# Expand dimensions since the model expects images to have shape: [1, None, None, 3]
# 	image_np_expanded = np.expand_dims(image_np, axis=0)
# 	# Actual detection.
# 	output_dict = run_inference_for_single_image(image_np, detection_graph)
# 	# Visualization of the results of a detection.
# 	vis_util.visualize_boxes_and_labels_on_image_array(
# 	    image_np,
# 	    output_dict['detection_boxes'],
# 	    output_dict['detection_classes'],
# 	    output_dict['detection_scores'],
# 	    category_index,
# 	    instance_masks=output_dict.get('detection_masks'),
# 	    use_normalized_coordinates=True,
# 	    line_thickness=8)
# 	plt.figure(figsize=IMAGE_SIZE)
# 	plt.imshow(image_np)
    
    
import cv2
cap = cv2.VideoCapture(0)
# Running the tensorflow session


#notification = n.notifications(time.strftime('%Y-%m-%d %H:%M:%S'),"gun")
#ns.save_notifications(notification)
fourcc = cv2.VideoWriter_fourcc(*'XVID')
out = cv2.VideoWriter('output.avi', fourcc, 20.0, (640, 480))
capture_duration = 0
with detection_graph.as_default():
  with tf.Session(graph=detection_graph) as sess:
   ret = True



   is_recording = False
   while (ret or is_recording == False):
      ret,image_np = cap.read()


      #check_if_recording = false

      #if():
       #sf.insert("video-records",)

      # Expand dimensions since the model expects images to have shape: [1, None, None, 3]
      image_np_expanded = np.expand_dims(image_np, axis=0)
      image_tensor = detection_graph.get_tensor_by_name('image_tensor:0')

      # Each box represents a part of the image where a particular object was detected.
      boxes = detection_graph.get_tensor_by_name('detection_boxes:0')
      # Each score represent how level of confidence for each of the objects.
      # Score is shown on the result image, together with the class label.
      scores = detection_graph.get_tensor_by_name('detection_scores:0')
      classes = detection_graph.get_tensor_by_name('detection_classes:0')
      num_detections = detection_graph.get_tensor_by_name('num_detections:0')





      # Actual detection.
      (boxes, scores, classes, num_detections) = sess.run(
          [boxes, scores, classes, num_detections],
          feed_dict={image_tensor: image_np_expanded})
      #print(boxes)
      # Visualization of the results of a detection.

      #if(display_name)
      vis_util.visualize_boxes_and_labels_on_image_array(
          image_np,
          np.squeeze(boxes),
          np.squeeze(classes).astype(np.int32),
          np.squeeze(scores),
          category_index,
          use_normalized_coordinates=True,
          line_thickness=8)
      #print(category_index)
#      plt.figure(figsize=IMAGE_SIZE)
#      plt.imshow(image_np)


      cv2.imshow('image',cv2.resize(image_np,(1280,720)))

      # print("classes", classes)

      if cv2.waitKey(25) & 0xFF == ord('q'):
          cv2.destroyAllWindows()
          cap.release()
          out.release()
          break


      threat = ([x for x in zip(classes[0],scores[0]) if x[1] >.5 ])
      if threat != None and len(threat) > 0:
        if threat[0] != None and len(threat[0]) > 0:
            categoryMap = category_index[threat[0][0]]
            categoryName = categoryMap['name']

            try:
                is_recording = True
                video_capture()
            except:
                print("failed during video capture process")


            try:
                notification = n.notifications(time.strftime('%Y-%m-%d %H:%M:%S'),categoryName)
                ns.save_notifications(notification)
            except:
                print("Failed during notifcation process")

            #if(threat[0] == category_index[1]):
                  #print(category_index[1].name)



      #for x in scores > .5:
         #if classes[x] == "gun" or "knives":


