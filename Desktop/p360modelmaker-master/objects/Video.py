class Video(object):
    id = 0
    filePath = ""

    def __init__(self, id, file_path):
        self.id = id;
        self.file_path = file_path;


def make_video(id, file_path):
    video = Video(id, file_path)
    return video
