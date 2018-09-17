#include "UsersInfoScene.h"
#include "network\HttpClient.h"
#include "json\document.h"
#include "json\writer.h" 
#include "json\stringbuffer.h" 
#include "Utils.h"

using namespace cocos2d::network;
using namespace rapidjson;
using namespace std;

cocos2d::Scene * UsersInfoScene::createScene() {
  return UsersInfoScene::create();
}

bool UsersInfoScene::init() {
  if (!Scene::init()) return false;

  auto visibleSize = Director::getInstance()->getVisibleSize();
  Vec2 origin = Director::getInstance()->getVisibleOrigin();

  auto getUserButton = MenuItemFont::create("Get User", CC_CALLBACK_1(UsersInfoScene::getUserButtonCallback, this));
  if (getUserButton) {
    float x = origin.x + visibleSize.width / 2;
    float y = origin.y + getUserButton->getContentSize().height / 2;
    getUserButton->setPosition(Vec2(x, y));
  }

  auto backButton = MenuItemFont::create("Back", [](Ref* pSender) {
    Director::getInstance()->popScene();
  });
  if (backButton) {
    float x = origin.x + visibleSize.width / 2;
    float y = origin.y + visibleSize.height - backButton->getContentSize().height / 2;
    backButton->setPosition(Vec2(x, y));
  }

  auto menu = Menu::create(getUserButton, backButton, NULL);
  menu->setPosition(Vec2::ZERO);
  this->addChild(menu, 1);

  limitInput = TextField::create("limit", "arial", 24);
  if (limitInput) {
    float x = origin.x + visibleSize.width / 2;
    float y = origin.y + visibleSize.height - 100.0f;
    limitInput->setPosition(Vec2(x, y));
    this->addChild(limitInput, 1);
  }

  messageBox = Label::create("", "arial", 30);
  if (messageBox) {
    float x = origin.x + visibleSize.width / 2;
    float y = origin.y + visibleSize.height / 2;
    messageBox->setPosition(Vec2(x, y));
    this->addChild(messageBox, 1);
  }

  return true;
}

void UsersInfoScene::getUserButtonCallback(Ref * pSender) {
  // Your code here
  // Your code here
	HttpRequest* request = new HttpRequest();
	request->setRequestType(HttpRequest::Type::GET);

	log("%s", limitInput->getString().c_str());
	request->setUrl("http://127.0.0.1:8000/users?limit="+limitInput->getString());
	request->setResponseCallback(CC_CALLBACK_2(UsersInfoScene::onGetUserResposeComplete, this));
	cocos2d::network::HttpClient::getInstance()->send(request);
	request->release();
}
void UsersInfoScene::onGetUserResposeComplete(HttpClient *sender, HttpResponse* response) {
	auto buffer = response->getResponseData();
	rapidjson::Document doc;
	doc.Parse(buffer->data(), buffer->size());
	if (doc["status"] == true) {
		rapidjson::Value &dataArray = doc["data"];
		string result="";
		if (dataArray.IsArray())
		{
			for (rapidjson::SizeType i = 0; i < dataArray.Size(); i++)
			{
				const rapidjson::Value& object = dataArray[i];
				result+="\nusername : ";
				result += object["username"].GetString();
				result += "\ndeck : ";
				auto &deckArray = object["deck"];
				for (rapidjson::SizeType j = 0; j < deckArray.Size(); j++) {
					for (auto &mem : deckArray[j].GetObjectW()) {
						result += "\n";
							result += mem.name.GetString();
						result += " : ";
						result += std::to_string(mem.value.GetInt());
					}
				}
			}
			this->messageBox->setString(result);
		}

	}
	else {
		this->messageBox->setString(std::string("Register Failed\n") + doc["msg"].GetString());
	}
}