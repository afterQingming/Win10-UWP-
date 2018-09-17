#pragma once

#ifndef __MODIFY_USER_SCENE_H__
#define __MODIFY_USER_SCENE_H__

#include "cocos2d.h"
#include "ui\CocosGUI.h"
#include "json\document.h"
#include "json\writer.h" 
#include "network\HttpClient.h"
#include "json\stringbuffer.h" 
#include "Utils.h"

using namespace cocos2d::network;
using namespace rapidjson;
using namespace std;
USING_NS_CC;
using namespace cocos2d::ui;
class ModifyUserScene : public cocos2d::Scene {
public:
  static cocos2d::Scene* createScene();

  virtual bool init();

  void putDeckButtonCallback(Ref *pSender);

  // implement the "static create()" method manually
  CREATE_FUNC(ModifyUserScene);

  Label *messageBox;
  TextField *deckInput;
  void onpPutDeckResposeComplete(HttpClient *sender, HttpResponse* response);
};

#endif
