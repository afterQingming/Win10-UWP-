#pragma once

#ifndef __LOGIN_REGISTER_SCENE_H__
#define __LOGIN_REGISTER_SCENE_H__
#include "network\HttpClient.h"
#include "cocos2d.h"
#include "ui\CocosGUI.h"
using namespace cocos2d::network;
using namespace cocos2d::ui;
USING_NS_CC;

class LoginRegisterScene : public cocos2d::Scene {
public:
  static cocos2d::Scene* createScene();

  virtual bool init();

  void loginButtonCallback(Ref *pSender);
  void registerButtonCallback(Ref *pSender);

  // implement the "static create()" method manually
  CREATE_FUNC(LoginRegisterScene);

  Label *messageBox;
private:
  TextField *usernameInput;
  TextField *passwordInput;
  void onLoginResposeComplete(HttpClient *sender, HttpResponse * response);
  void onRegisterResposeComplete(HttpClient *sender, HttpResponse* response);
};

#endif // !__LOGIN_REGISTER_SCENE_H__
