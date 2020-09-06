from os.path import join

from flask_jwt_extended import create_access_token, create_refresh_token, jwt_refresh_token_required, get_jwt_identity, \
    jwt_required

from header import *
class User(db.Model):
    id=db.Column(db.Integer,primary_key=True)
    public_id=db.Column(db.String(100),unique=True)
    name=db.Column(db.String(100))
    password=db.Column(db.String(100))
    admin=db.Column(db.Boolean)
    # address=db.Column(db.String(100))
    @staticmethod
    def Index():
        db.create_all()
        message={"message":"Table Created"}
        return jsonify(message)
    @staticmethod
    @jwt_required
    def CreateUser():
        data=request.get_json()
        objUser=User()
        objUser.public_id=str(uuid.uuid4())
        objUser.name=data['name']
        objUser.password=generate_password_hash(data['password'],method="sha256")
        objUser.admin=False
        db.session.add(objUser)
        db.session.commit()
        message=[]
        message_dict={"message":"User created"}
        message.append(message_dict)
        return  json.dumps(message)
    @staticmethod
    @jwt_required
    def GetUserAll():
        obUser=User.query.all()
        result=[]
        for col in obUser:
            user_dict={}
            user_dict["id"]=col.id
            user_dict["public_id"] = col.public_id
            user_dict["name"] = col.name
            user_dict["password"] = col.password
            user_dict["admin"] = col.admin
            result.append(user_dict)
        return  json.dumps(result)
    @staticmethod
    def deleteByPubliceId(public_id):
        objUser=User.query.filter_by(public_id=public_id).first()
        if not objUser:
            message = []
            message_dict = {"message": "Not found"}
            message.append(message_dict)
            return json.dumps(message)
        db.session.delete(objUser)
        db.session.commit()
        message = []
        message_dict = {"message": "Delete User"}
        message.append(message_dict)
        return json.dumps(message)
    @staticmethod
    def searchByName():
        data=request.get_json()
        objUser=User.query.filter_by(name=data['name']).all()
        if not objUser:
            message={"message":"Not found user"}
            return  jsonify(message)
        result = []
        for col in objUser:
            user_dict = {}
            user_dict["id"] = col.id
            user_dict["public_id"] = col.public_id
            user_dict["name"] = col.name
            user_dict["password"] = col.password
            user_dict["admin"] = col.admin
            result.append(user_dict)
        return json.dumps(result)
    @staticmethod
    def Login():
        User.message = []
        data = request.get_json()
        objUser1 = User()
        objUser1.name = data["name"]
        objUser1.password = data["password"]
        objUser = User.query.filter_by(name=objUser1.name, admin=True).first()
        if not objUser:
            User.message_dict = {"message": "Username and password is invalid"}
            User.message.append(User.message_dict)
            return json.dumps(User.message), 401
        if not check_password_hash(objUser.password, objUser1.password):
            User.message_dict = {"message": "Username and password is invalid"}
            User.message.append(User.message_dict)
            return json.dumps(User.message), 401
        result = []
        ret = {
            'access_token': create_access_token(identity=objUser1.public_id),
            'refresh_token': create_refresh_token(identity=objUser1.public_id)
        }
        result.append(ret)
        return json.dumps(result), 200

    @staticmethod
    # @jwt_required
    def PromoteUser(public_id):

        data = request.get_json()
        objUser = User.query.filter_by(public_id=public_id).first()
        if not objUser:
            message = []
            message_dict = {"message": "Not found"}
            message.append(message_dict)
            return json.dumps(message)
        objUser.admin = data['admin']
        db.session.add(objUser)
        db.session.commit()
        message = []
        message_dict = {"message": "Promoted"}
        message.append(message_dict)
        return json.dumps(message)
    @staticmethod
    @jwt_refresh_token_required
    def refresh():
        current_user = get_jwt_identity()
        ret = {
            'access_token': create_access_token(identity=current_user)
        }
        return jsonify(ret), 200

    @staticmethod
    @jwt_required
    def protected():
        username = get_jwt_identity()
        return jsonify(logged_in_as=username), 200





