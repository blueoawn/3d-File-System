import psycopg2
import datetime
import time




conn = psycopg2.connect("dbname=postgres user=ronitghosh password= host=localhost")


def save_video(video):
    cur = conn.cursor()

    cur.execute("CREATE sequence vid_sequence START WITH 7 INCREMENT BY 1 MINVALUE 1 MAXVALUE 1000000000")
    cur.execute("select nextval('vid_sequence')")
    result = cur.fetchone()
    cur.execute("INSERT INTO videos(id, video) VALUES(%s,)",(result,video))
    conn.commit()





def save_notifications(notification):
    cur = conn.cursor()
    timeStamp = notification.timeStamp
    threat = notification.threat
    #cur.execute("CREATE sequence not_sequence START WITH 7 INCREMENT BY 1 MINVALUE 1 MAXVALUE 1000000000")

    cur.execute("select nextval('not_sequence')")
    result = cur.fetchone()
    cur.execute("INSERT INTO notifications(id,timestamp,threat) VALUES(%s, %s, %s)",(result,timeStamp,threat))
    conn.commit()
    close_connection(cur, conn)


def close_connection(cur, conn):
    cur.close()
    del cur
    conn.close()

