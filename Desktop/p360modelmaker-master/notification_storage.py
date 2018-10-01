import psycopg2




conn = psycopg2.connect("dbname=postgres user=ronitghosh password= host=localhost")
print(conn)
cur = conn.cursor()



#cur.execute("INSERT INTO notifications (id,timestamp,threat) VALUES(0,?, )")


