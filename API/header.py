from flask import Flask, request, jsonify, make_response, json
from flask_jwt_extended import JWTManager
from flask_migrate import Migrate, MigrateCommand
from flask_script import Manager
from flask_sqlalchemy import SQLAlchemy
from werkzeug.security import generate_password_hash,check_password_hash
import uuid
import jwt
import datetime
from functools import wraps
from flask_cors import CORS
app = Flask(__name__)
cors = CORS(app)
# app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///app.db'
app.config[ 'SQLALCHEMY_DATABASE_URI']="mysql://root:admin@localhost/app1"
app.config['SQLALCHEMY_TRACK_MODIFICATIONS']=True
app.config['SECRET_KEY'] = 'thisissecret'
db=SQLAlchemy(app)
manager = Manager(app)
migrate = Migrate(app, db)
manager.add_command('db', MigrateCommand)
jwt = JWTManager(app)

