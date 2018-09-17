#include "Thunder.h"
#include <stdio.h>
#include <algorithm>

USING_NS_CC;

using namespace CocosDenshion;
static int ct = 0;
static int dir = 4;
Scene* Thunder::createScene() {
	// 'scene' is an autorelease object
	auto scene = Scene::create();

	// 'layer' is an autorelease object
	auto layer = Thunder::create();

	// add layer as a child to scene
	scene->addChild(layer);

	// return the scene
	return scene;
}

bool Thunder::init() {
	if (!Layer::init()) {
		return false;
	}
	stoneType = 0;
	isMove = false;  // 是否点击飞船
	visibleSize = Director::getInstance()->getVisibleSize();

	// 创建背景
	auto bgsprite = Sprite::create("bg.jpg");
	bgsprite->setPosition(visibleSize / 2);
	bgsprite->setScale(visibleSize.width / bgsprite->getContentSize().width,
		visibleSize.height / bgsprite->getContentSize().height);
	this->addChild(bgsprite, 0);

	// 创建飞船
	player = Sprite::create("player.png");
	player->setAnchorPoint(Vec2(0.5, 0.5));
	player->setPosition(visibleSize.width / 2, player->getContentSize().height);
	player->setName("player");
	this->addChild(player, 1);

	// 显示陨石和子弹数量
	enemysNum = Label::createWithTTF("enemys: 0", "fonts/arial.TTF", 20);
	enemysNum->setColor(Color3B(255, 255, 255));
	enemysNum->setPosition(50, 60);
	this->addChild(enemysNum, 3);
	bulletsNum = Label::createWithTTF("bullets: 0", "fonts/arial.TTF", 20);
	bulletsNum->setColor(Color3B(255, 255, 255));
	bulletsNum->setPosition(50, 30);
	this->addChild(bulletsNum, 3);

	addEnemy(5);        // 初始化陨石
	preloadMusic();     // 预加载音乐
	playBgm();          // 播放背景音乐
	explosion();        // 创建爆炸帧动画

	// 添加监听器
	addTouchListener();
	addKeyboardListener();
	addCustomListener();

	// 调度器
	schedule(schedule_selector(Thunder::update), 0.04f, kRepeatForever, 0);

	return true;
}

//预加载音乐文件
void Thunder::preloadMusic() {
	// Todo
	auto audio = SimpleAudioEngine::getInstance();
	audio->preloadBackgroundMusic("music/bgm.mp3");
	audio->preloadEffect("music/explore.wav");
	audio->preloadEffect("music/fire.wav");
}

//播放背景音乐
void Thunder::playBgm() {
	// Todo
	SimpleAudioEngine::getInstance()->playBackgroundMusic("music/bgm.mp3", true);
}

//初始化陨石
void Thunder::addEnemy(int n) {
	enemys.clear();
	for (unsigned i = 1; i <= 3; ++i) {
		char enemyPath[20];
		sprintf(enemyPath, "stone%d.png", i);
		double width = visibleSize.width / (n + 1.0),
			height = visibleSize.height - (50 * (i ));
		for (int j = 0; j < n; ++j) {
			auto enemy = Sprite::create(enemyPath);
			enemy->setAnchorPoint(Vec2(0.5, 0.5));
			enemy->setScale(0.5, 0.5);
			enemy->setPosition(width * (j + 1), height);
			enemys.push_back(enemy);
			addChild(enemy, 1);
		}
	}
}

// 陨石向下移动并生成新的一行(加分项)
void Thunder::newEnemy() {
	// Todo
	double width = visibleSize.width /6,
		height = visibleSize.height - 50;
	for (Sprite* s : enemys) {
		if (s != NULL) {
			s->setPosition(s->getPosition() + Vec2(0, -50));
		}
	}
	char enemyPath[20];
	sprintf(enemyPath, "stone%d.png", 3 - stoneType);
	stoneType++;
	stoneType %= 3;
	for (int j = 0; j < 5; ++j) {
		auto enemy = Sprite::create(enemyPath);
		enemy->setAnchorPoint(Vec2(0.5, 0.5));
		enemy->setScale(0.5, 0.5);
		enemy->setPosition(width * (j+0.5), height);
		enemys.push_back(enemy);
		addChild(enemy, 1);
	}

}

// 移动飞船
void Thunder::movePlane(char c) {
	// Todo
	switch (c) {
		case 'A':if (player->getPosition().x>50)  player->runAction(MoveBy::create(0.5, Vec2(-10, 0))); break;
		case 'D':if (visibleSize.width- player->getPosition().x>50)  player->runAction(MoveBy::create(0.5, Vec2(10, 0))); break;
		default:break;
	}
		

}

//发射子弹
void Thunder::fire() {
	auto bullet = Sprite::create("bullet.png");
	bullet->setAnchorPoint(Vec2(0.5, 0.5));
	bullets.push_back(bullet);
	bullet->setPosition(player->getPosition());
	addChild(bullet, 1);
	SimpleAudioEngine::getInstance()->playEffect("music/fire.wav", false);

	bullet->runAction(Sequence::create(MoveBy::create(3, Vec2(0, visibleSize.height)), CallFunc::create([=] {bullets.remove(bullet); }), RemoveSelf::create(true), nullptr));
	// 移除飞出屏幕外的子弹
	// Todo

}

// 切割爆炸动画帧
void Thunder::explosion() {
	// Todo
	auto texture = Director::getInstance()->getTextureCache()->addImage("explosion.png");
	explore.reserve(8);
	for (int i = 0,j=0; i < 8; i++) {
		if (i == 5) {
			j++;
		}
		auto frame = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(i % 5*189, j * 210, 189, 210)));
		explore.pushBack(frame);
		
		
	}

}

void Thunder::update(float f) {
	// 实时更新页面内陨石和子弹数量(不得删除)
	// 要求数量显示正确(加分项)
	char str[15];
	sprintf(str, "enemys: %d", enemys.size());
	enemysNum->setString(str);
	sprintf(str, "bullets: %d", bullets.size());
	bulletsNum->setString(str);

	// 飞船移动
	if (isMove)
		this->movePlane(movekey);

	
	++ct;
	if (ct == 120)
		ct = 40, dir = -dir;
	else if (ct == 80) {
		dir = -dir;
		newEnemy();  // 陨石向下移动并生成新的一行(加分项)
	}
	else if (ct == 20)
		ct = 40, dir = -dir;

	//陨石左右移动
	for (Sprite* s : enemys) {
		if (s != NULL) {
			s->setPosition(s->getPosition() + Vec2(dir, 0));
		}
	}

	// 分发自定义事件
	EventCustom e("meet");
	EventCustom e1("meet1");
	_eventDispatcher->dispatchEvent(&e);
	_eventDispatcher->dispatchEvent(&e1);
}

// 自定义碰撞事件
void Thunder::meet(EventCustom * event) {
	// 判断子弹是否打中陨石并执行对应操作
	// Todo
	for each(auto bullet in bullets) {
		for each(auto enemy in enemys){

			if ((enemy->getPosition() - bullet->getPosition()).getLength()< Vec2(25, 25).getLength()) {
				
				auto blt = bullet;
				auto enm = enemy;
				SimpleAudioEngine::getInstance()->playEffect("music/explore.wav", false);
				enm->runAction(
					Sequence::create(
						Animate::create(
							Animation::createWithSpriteFrames(explore, 0.05f, 1)
						),
						CallFunc::create(
							[enm] {
					enm->removeFromParentAndCleanup(true);
					
				}
						),
						nullptr
					)
				);
				blt->removeFromParentAndCleanup(false);
				bullets.remove(bullet);
				enemys.remove(enemy);
				return;
			}
		}
	}
}
void Thunder::meet1(EventCustom* event) {
	int i = 0;
	bool flag = false;
	for (auto tar : enemys) {
		if (tar->getPosition().y < 20)
			flag = true;
		else if ((tar->getPosition() - player->getPosition()).getLength() < Vec2(30, 30).getLength())
			flag = true;
		i++;
		if (i ==20||flag)
			break;
	}
	if(flag)
	stopAc();
	
}





void Thunder::stopAc() {
    // Todo
	player->runAction(
		Sequence::create(
			Animate::create(
				Animation::createWithSpriteFrames(explore, 0.05f, 1)
			),
			CallFunc::create(
				[=] {
		player->removeFromParentAndCleanup(true);

	}
			),
			nullptr
		)
	);
	for each(auto i in bullets) {
		i->stopAllActions();
	}
	auto over = Sprite::create("gameOver.png");
	addChild(over);
	over->setAnchorPoint(Vec2(0.5, 0.5));
	over->setPosition(Vec2(visibleSize.width / 2, visibleSize.height / 2));
	_eventDispatcher->removeAllEventListeners();
	this->unscheduleAllSelectors();
	
}



// 添加自定义监听器
void Thunder::addCustomListener() {
	// Todo
	auto meetListener =
		EventListenerCustom::create("meet", CC_CALLBACK_1(Thunder::meet, this));
	auto meetListener1 =
		EventListenerCustom::create("meet1", CC_CALLBACK_1(Thunder::meet1, this));
	_eventDispatcher->addEventListenerWithSceneGraphPriority(meetListener,this);
	_eventDispatcher->addEventListenerWithSceneGraphPriority(meetListener1, this);
}

// 添加键盘事件监听器
void Thunder::addKeyboardListener() {
	// Todo
	auto KeyboardListener = EventListenerKeyboard::create();
	KeyboardListener->onKeyPressed = CC_CALLBACK_2(Thunder::onKeyPressed, this);
	KeyboardListener->onKeyReleased= CC_CALLBACK_2(Thunder::onKeyReleased, this);
	this->getEventDispatcher()->addEventListenerWithSceneGraphPriority(KeyboardListener, this);

}

void Thunder::onKeyPressed(EventKeyboard::KeyCode code, Event* event) {
	switch (code) {
	case EventKeyboard::KeyCode::KEY_LEFT_ARROW:
	case EventKeyboard::KeyCode::KEY_CAPITAL_A:
	case EventKeyboard::KeyCode::KEY_A:
		movekey = 'A';
		isMove = true;
		break;
	case EventKeyboard::KeyCode::KEY_RIGHT_ARROW:
	case EventKeyboard::KeyCode::KEY_CAPITAL_D:
	case EventKeyboard::KeyCode::KEY_D:
		movekey = 'D';
		isMove = true;
		break;
	case EventKeyboard::KeyCode::KEY_SPACE:
		fire();
		break;
	}
}

void Thunder::onKeyReleased(EventKeyboard::KeyCode code, Event* event) {
	switch (code) {
	case EventKeyboard::KeyCode::KEY_LEFT_ARROW:
	case EventKeyboard::KeyCode::KEY_A:
	case EventKeyboard::KeyCode::KEY_CAPITAL_A:
	case EventKeyboard::KeyCode::KEY_RIGHT_ARROW:
	case EventKeyboard::KeyCode::KEY_D:
	case EventKeyboard::KeyCode::KEY_CAPITAL_D:
		isMove = false;
		break;
	}
}

// 添加触摸事件监听器
void Thunder::addTouchListener() {
	// Todo
	auto touchListener = EventListenerTouchOneByOne::create();
	touchListener->onTouchMoved = CC_CALLBACK_2(Thunder::onTouchMoved, this);
	touchListener->onTouchBegan = CC_CALLBACK_2(Thunder::onTouchBegan, this);
	touchListener->onTouchEnded = CC_CALLBACK_2(Thunder::onTouchEnded, this);
	this->getEventDispatcher()->addEventListenerWithSceneGraphPriority(touchListener, this);
}

// 鼠标点击发射炮弹
bool Thunder::onTouchBegan(Touch *touch, Event *event) {
	if (touch->getLocation().getDistance(player->getPosition()) <= 30)
        isClick = true;
    // Todo
	fire();
	return true;
}

void Thunder::onTouchEnded(Touch *touch, Event *event) {
	isClick = false;
}

// 当鼠标按住飞船后可控制飞船移动 (加分项)
void Thunder::onTouchMoved(Touch *touch, Event *event) {
	// Todo
	if (!isClick)
		return;
	Vec2 delta = touch->getDelta();
	if(delta.x<0&& player->getPosition().x>30)
		player->runAction(MoveBy::create(0.1f, Vec2(delta.x,0)));
	else if (delta.x>0 && visibleSize.width-player->getPosition().x>30)
		player->runAction(MoveBy::create(0.1f, Vec2(delta.x, 0)));
}
