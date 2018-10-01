import psycopg2


conn = psycopg2.connect("dbname=modelmaker user=postgres password=rainbow host=localhost")
cur = conn.cursor()



#class SaveBlob(APIView):


 #   def post(self, request):
  #      with open("file.webm", "wb") as vid:
   #         video_stream = request.FILES['blob'].read()
    #        vid.write(video_stream)
     #       return Response()

def post_video(video):


    cur.execute("INSERT INTO videos () ")


