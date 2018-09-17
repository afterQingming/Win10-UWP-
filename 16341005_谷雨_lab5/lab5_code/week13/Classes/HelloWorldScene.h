#pragma once
#include "cocos2d.h"
#include "Monster.h"
#define database	UserDefault::getInstance()
using namespace cocos2d;

class HelloWorld : public cocos2d::Scene
{
public:
    static cocos2d::Scene* createScene();

    virtual bool init();
	void AttackAction(cocos2d::Label* label);
	void DieAction();
	void RunAction(cocos2d::Label* label);
	void updateTime(float time);
	void hitByMonster(float dt) {
		auto fac = Factory::getInstance();
		Sprite* collision = fac->collider(player->getBoundingBox());
		if(collision != NULL) {
			//
			fac->removeMonster(collision);
			if (pT->getPercentage() > 0)
			{
				pT->runAction(ProgressTo::create(2, pT->getPercentage() - 20));
			}
			if (pT->getPercentage() <= 0) {

				IsAlive = false;
				DieAction();
				unschedule(schedule_selector(HelloWorld::updateTime));
				unschedule(schedule_selector(HelloWorld::hitByMonster));

			}
		}
	}
	void HitMonster() {
		auto fac = Factory::getInstance();
		Rect playerRect = player->getBoundingBox();
		Rect attackRect = Rect(playerRect.getMidX() - 40,
			playerRect.getMinY(),
			playerRect.getMaxX() - playerRect.getMinX() + 80,
			playerRect.getMaxY() - playerRect.getMinY());
		if (!toward) {
			attackRect = Rect(playerRect.getMidX() - 80,
				playerRect.getMinY(),
				playerRect.getMaxX() - playerRect.getMinX() + 80,
				playerRect.getMaxY() - playerRect.getMinY());
		}
		Sprite* collision = fac->collider(attackRect);
		if (collision != NULL) {
			//
			fac->removeMonster(collision);
			if (pT->getPercentage() < 100)
			{
				pT->runAction(ProgressTo::create(2, pT->getPercentage() + 20));
			}
			dcount++;
			count->setString(std::to_string(dcount));
			if (database->isXMLFileExist()) {

				log("exist");
				log(database->getXMLFilePath().c_str());
				database->setIntegerForKey("value", dcount);
				database->flush();
			}
			else {
				log("notexist");
			}
			
		}
	}
	void getMonster() {
		auto fac = Factory::getInstance();
		auto m = fac->createMonster();
		float x = random(origin.x, visibleSize.width);
		float y = random(origin.y, visibleSize.height);
		m->setPosition(x, y);
		addChild(m, 3);
	}
	void moveMonster() {
		auto pos = player->getPosition();
		auto fac = Factory::getInstance();
		fac->moveMonster(pos, 1);
	}
	void createMap() {
		TMXTiledMap * tmx = TMXTiledMap::create("map.tmx");
		tmx->setPosition(visibleSize.width / 2, visibleSize.height / 2);
		tmx->setAnchorPoint(Vec2(0.5, 0.5));
		tmx->setScale(Director::getInstance()->getContentScaleFactor());
		log("%lf,%lf", tmx->getContentSize().width, tmx->getContentSize().height);
		this->addChild(tmx, 0);
	}
	bool IsAlive = true;
	int get() {		int value=database->getIntegerForKey("value");

		CCLOG("%ldhahahaha",value);
		return value;
	}
    // implement the "static create()" method manually
    CREATE_FUNC(HelloWorld);
private:
	cocos2d::Sprite* player;
	cocos2d::Vector<SpriteFrame*> attack;
	cocos2d::Vector<SpriteFrame*> die;
	cocos2d::Vector<SpriteFrame*> run;
	cocos2d::Vector<SpriteFrame*> idle;
	cocos2d::Size visibleSize;
	cocos2d::Vec2 origin;
	cocos2d::Label* time;
	cocos2d::Label* count;
	int dtime=180;
	int dcount;
	bool toward = true;
	cocos2d::ProgressTimer* pT;
};
