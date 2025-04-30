from flask import Flask, request, jsonify
from flask_cors import CORS
import mysql.connector

app = Flask(__name__)
CORS(app)  # Erlaubt Anfragen von Unity

# Verbindung zur MySQL-Datenbank
db_config = {
    'host': 'host.docker.internal',
    #'host': 'localhost',
    'user': 'root',
    'password': 'mysqlpassword',
    'database': 'advprog'
}

@app.route('/register', methods=['POST'])
def register():
    username = request.form.get('username')
    email = request.form.get('email')
    password = request.form.get('password')

    if not username or not email or not password:
        return jsonify({'status': 'missing'})

    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor()

    # prüfen ob username oder email existiert
    cursor.execute("SELECT * FROM users WHERE username = %s OR email = %s", (username, email))
    if cursor.fetchone():
        return jsonify({'status': 'exists'})

    # in Datenbank einfügen
    cursor.execute("INSERT INTO users (username, email, password) VALUES (%s, %s, %s)",
                   (username, email, password))
    conn.commit()
    conn.close()

    return jsonify({'status': 'ok'})


@app.route('/login', methods=['POST'])
def login():
    username = request.form['username']
    password = request.form['password']

    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM users WHERE username = %s AND password = %s", (username, password))
    result = cursor.fetchone()
    conn.close()

    if result:
        return jsonify({'status': 'ok'})
    else:
        return jsonify({'status': 'fail'})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
