from  header import *
from Classes.User import *
from Classes.Tags import *

import pymysql
pymysql.install_as_MySQLdb()

#route
app.add_url_rule("/",view_func=User.Index)
app.add_url_rule("/user",view_func=User.CreateUser,methods=["POST"])
app.add_url_rule("/user",view_func=User.GetUserAll,methods=["GET"])
app.add_url_rule("/user/<public_id>",view_func=User.deleteByPubliceId,methods=["DELETE"])
app.add_url_rule("/user/name",view_func=User.searchByName,methods=["GET"])
app.add_url_rule("/login",view_func=User.Login,methods=["POST"])
app.add_url_rule("/user/promote/<public_id>",view_func=User.PromoteUser,methods=["PUT"])


#Tag
# app.add_url_rule("/tag",view_func=Tag.Index)
# app.add_url_rule("/tag/create",view_func=Tag.CreateTag,methods=["POST"])
# app.add_url_rule("/tag/get",view_func=Tag.GetTagAll,methods=["GET"])
# app.add_url_rule("/tag/delete/<id>",view_func=Tag.deleteById,methods=["DELETE"])
# app.add_url_rule("/tag/name",view_func=Tag.searchByName,methods=["GET"])



