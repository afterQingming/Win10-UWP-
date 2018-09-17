#include "GameScene.h"

USING_NS_CC;

Scene* GameSence::createScene()
{
	return GameSence::create();
}

// on "init" you need to initialize your instance
bool GameSence::init()
{
	//////////////////////////////
	// 1. super init first
	if (!Scene::init())
	{
		return false;
	}

	auto visibleSize = Director::getInstance()->getVisibleSize();
	Vec2 origin = Director::getInstance()->getVisibleOrigin();

	//add touch listener
	EventListenerTouchOneByOne* listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);
	listener->onTouchBegan = CC_CALLBACK_2(GameSence::onTouchBegan, this);
	Director::getInstance()->getEventDispatcher()->addEventListenerWithSceneGraphPriority(listener, this);

	auto background = Sprite::create("level-background-0.jpg");
	background->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y));
	this->addChild(background, 0);

	
	 shoot = Label::createWithSystemFont("shoot", "arial", 60);
	shoot->setPosition(Vect(visibleSize.width / 2 + 300, visibleSize.height / 2 + origin.y + 150));
	this->addChild(shoot, 1);

	 stoneLayer = Layer::create();
	stoneLayer->setPosition(0, 0);
	this->addChild(stoneLayer, 1);
	
	 stone = Sprite::create("stone.png");
	stoneLayer->addChild(stone, 0);
	stone->setPosition(Vec2(visibleSize.width / 2, visibleSize.height / 2 + origin.y + 150));

	 mouseLayer = Layer::create();
	this->addChild(mouseLayer, 1);
	mouseLayer->setPosition(0, 0);
	
	

	 mouse = Sprite::createWithSpriteFrameName("mouse-0.png");
	mouseLayer->addChild(mouse, 1);
	mouse->setPosition(Vec2(visibleSize.width / 2, visibleSize.height / 2 + origin.y));
	Animate* mouseAnimate = Animate::create(AnimationCache::getInstance()->getAnimation("mouseAnimation"));
	mouse->runAction(RepeatForever::create(mouseAnimate));
	
	

	return true;
}

bool GameSence::onTouchBegan(Touch *touch, Event *unused_event) {

	auto visibleSize_ = Director::getInstance()->getVisibleSize();
	Vec2 origin_ = Director::getInstance()->getVisibleOrigin();

	auto location = touch->getLocation();

	if(shoot->boundingBox().containsPoint(location)){

		//石头缩放
		shoot->runAction(Sequence::create(ScaleTo::create(0.2, 1.5), ScaleTo::create(0.2, 1), nullptr));

		//老鼠跑
		auto mouseNewPosition = Vec2(CCRANDOM_0_1() * visibleSize_.width, CCRANDOM_0_1() * visibleSize_.height);
		auto mouseMoveTo = MoveTo::create(0.5, mouseNewPosition);
		mouse->runAction(mouseMoveTo);

		//石头射
		auto bullet = Sprite::create("stone.png");
		bullet->setPosition(Vec2(visibleSize_.width / 2, visibleSize_.height / 2 + origin_.y + 150));
		this->addChild(bullet, 1);
		bullet->runAction(Sequence::create(MoveTo::create(0.3, mouse->getPosition()), FadeOut::create(0.5), nullptr));
		
		//钻石留下 闪闪
		auto diamond = Sprite::createWithSpriteFrameName("pulled-diamond-0.png");
		Animate* diamondAnimate = Animate::create(AnimationCache::getInstance()->getAnimation("diamondAnimation"));
		diamond->runAction(RepeatForever::create(diamondAnimate));
		diamond->setPosition(mouse->getPosition());
		this->addChild(diamond, 1);


		



	}
	else {
		auto cheese = Sprite::create("cheese.png");
		this->addChild(cheese, 1);
		cheese->setPosition(location);

		cheese->runAction(Sequence::create(FadeOut::create(1.0), nullptr));


		mouse->runAction(MoveTo::create(1.0, location));
	}



	


	return true;
}

