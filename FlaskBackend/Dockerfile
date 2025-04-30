
FROM python:3.12-slim


WORKDIR /app

# Kopiere alle Dateien in den Container
COPY . .

# Installiere die Abhängigkeiten aus requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# gibt den port an
EXPOSE 5000

# Starte die Flask-App
CMD ["python", "main.py"]