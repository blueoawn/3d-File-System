import boto3

s3 = boto3.resource('s3')
# Let's use Amazon S3

'''
def save_video_to_file_storage():
	buckets = []
	for bucket in s3.buckets.all():
		buckets.append(bucket.name)
	print(buckets)
	if('video-records' not in buckets):
		s3.create_bucket(Bucket='video-records')
'''


def insert(bucket,folder,filename):
	s3.Object(bucket, folder+filename).put(Body=open(filename, 'rb'))



#insert('video-records','folder2/insdie/','sample2.txt')
