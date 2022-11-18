
from string import ascii_lowercase
import json

dataset = []


for c in ascii_lowercase:
    obj = {"name": c.capitalize(), "value": ord(c.capitalize())}
    dataset.append(obj)

for i in range(ord('0'), ord('9'), 1):
    obj = {"name": chr(i), "value": i}
    dataset.append(obj)

for i in range(12):
    obj = {"name": "F" + str(i + 1), "value": i + 112}
    dataset.append(obj)

dataset.append({"name": "UP", "value": 38});
dataset.append({"name": "DOWN", "value": 40});

jsonObj = json.dumps(dataset);

with open("C:\\Users\\Martin Belej\\source\\repos\\C#\\VDJ Controller\\VDJ Controller\\bin\\Debug\\keys.json", "w") as outfile:
    outfile.write(jsonObj)