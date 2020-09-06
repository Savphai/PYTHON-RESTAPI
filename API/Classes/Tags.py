from os.path import join

from flask_jwt_extended import create_access_token, create_refresh_token, jwt_refresh_token_required, get_jwt_identity, \
    jwt_required

from header import *
class Tag(db.Model):
    id=db.Column(db.Integer,primary_key=True)
    name=db.Column(db.String(100))
    @staticmethod
    def Index():
        db.create_all()
        message={"message":"Table Created"}
        return jsonify(message)
    @staticmethod
    @jwt_required
    def CreateTag():
        data=request.get_json()
        objTag=Tag()
        objTag.name=data['name']
        db.session.add(objTag)
        db.session.commit()
        message=[]
        message_dict={"message":"Tag created"}
        message.append(message_dict)
        return  json.dumps(message)
    @staticmethod
    @jwt_required
    def GetTagAll():
        obTag=Tag.query.all()
        result=[]
        for col in obTag:
            user_dict={}
            user_dict["id"]=col.id
            user_dict["name"] = col.name
            result.append(user_dict)
        return  json.dumps(result)
    @staticmethod
    def deleteById(id):
        objTag=Tag.query.filter_by(id=id).first()
        if not objTag:
            message = []
            message_dict = {"message": "Not found"}
            message.append(message_dict)
            return json.dumps(message)
        db.session.delete(objTag)
        db.session.commit()
        message = []
        message_dict = {"message": "Delete Tag"}
        message.append(message_dict)
        return json.dumps(message)
    @staticmethod
    def searchByName():
        data=request.get_json()
        objTag=Tag.query.filter_by(name=data['name']).all()
        if not objTag:
            message={"message":"Not found Tag"}
            return  jsonify(message)
        result = []
        for col in objTag:
            user_dict = {}
            user_dict["id"] = col.id
            user_dict["name"] = col.name
            result.append(user_dict)
        return json.dumps(result)
    
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