#include "HelloWorldScene.h"
#include "SimpleAudioEngine.h"
#pragma execution_character_set("utf-8")

USING_NS_CC;

Scene* HelloWorld::createScene()
{
    return HelloWorld::create();
}

// Print useful error message instead of segfaulting when files are not there.
static void problemLoading(const char* filename)
{
    printf("Error while loading: %s\n", filename);
    printf("Depending on how you compiled you might have to add 'Resources/' in front of filenames in HelloWorldScene.cpp\n");
}

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
    //////////////////////////////
    // 1. super init first
    if ( !Scene::init() )
    {
        return false;
    }

    visibleSize = Director::getInstance()->getVisibleSize();
    origin = Director::getInstance()->getVisibleOrigin();

	//创建一张贴图
	auto texture = Director::getInstance()->getTextureCache()->addImage("$lucia_2.png");
	//从贴图中以像素单位切割，创建关键帧
	auto frame0 = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(0, 0, 113, 113)));
	//使用第一帧创建精灵
	player = Sprite::createWithSpriteFrame(frame0);
	player->setPosition(Vec2(origin.x + visibleSize.width / 2,
		origin.y + visibleSize.height / 2));
	addChild(player, 3);

	//hp条
	Sprite* sp0 = Sprite::create("hp.png", CC_RECT_PIXELS_TO_POINTS(Rect(0, 320, 420, 47)));
	Sprite* sp = Sprite::create("hp.png", CC_RECT_PIXELS_TO_POINTS(Rect(610, 362, 4, 16)));

	//使用hp条设置progressBar
	pT = ProgressTimer::create(sp);
	pT->setScaleX(90);
	pT->setAnchorPoint(Vec2(0, 0));
	pT->setType(ProgressTimerType::BAR);
	pT->setBarChangeRate(Point(1, 0));
	pT->setMidpoint(Point(0, 1));
	pT->setPercentage(100);
	pT->setPosition(Vec2(origin.x + 14 * pT->getContentSize().width, origin.y + visibleSize.height - 2 * pT->getContentSize().height));
	addChild(pT, 1);
	sp0->setAnchorPoint(Vec2(0, 0));
	sp0->setPosition(Vec2(origin.x + pT->getContentSize().width, origin.y + visibleSize.height - sp0->getContentSize().height));
	addChild(sp0, 0);

	//时间
	time = Label::createWithTTF(std::to_string(dtime), "fonts/arial.ttf", 36);
	time->setPosition(Vec2(origin.x + visibleSize.width / 2,
		origin.y + visibleSize.height - time->getContentSize().height));

	// add the label as a child to this layer
	this->addChild(time, 1);

	schedule(schedule_selector(HelloWorld::updateTime), 1.0f, kRepeatForever, 0);

	// 静态动画
	idle.reserve(1);
	idle.pushBack(frame0);

	// 攻击动画
	attack.reserve(17);
	for (int i = 0; i < 17; i++) {
		auto frame = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(113 * i, 0, 113, 113)));
		attack.pushBack(frame);
	}
	

	// 死亡动画(帧数：22帧，高：90，宽：79）
	auto texture2 = Director::getInstance()->getTextureCache()->addImage("$lucia_dead.png");
	die.reserve(22);
	for (int i = 0; i < 22; i++) {
		auto frame = SpriteFrame::createWithTexture(texture2, CC_RECT_PIXELS_TO_POINTS(Rect(79 * i, 0, 79, 90)));
		die.pushBack(frame);
	}


	// 运动动画(帧数：8帧，高：101，宽：68）
	auto texture3 = Director::getInstance()->getTextureCache()->addImage("$lucia_forward.png");
	run.reserve(8);
	for (int i = 0; i < 8; i++) {
		auto frame = SpriteFrame::createWithTexture(texture3, CC_RECT_PIXELS_TO_POINTS(Rect(68 * i, 0, 68, 101)));
		run.pushBack(frame);
	}

	auto W = Label::createWithTTF("W","fonts/arial.ttf",36);
	auto A = Label::createWithTTF("A","fonts/arial.ttf",36);
	auto S = Label::createWithTTF("S","fonts/arial.ttf",36);
	auto D = Label::createWithTTF("D","fonts/arial.ttf",36);
	auto X = Label::createWithTTF("X","fonts/arial.ttf",36);
	auto Y = Label::createWithTTF("Y","fonts/arial.ttf",36);
	W->setName("W");
	A->setName("A");
	S->setName("S");
	D->setName("D");
	X->setName("X");
	Y->setName("Y");




	auto Btn_W = MenuItemLabel::create(W, CC_CALLBACK_0(HelloWorld::RunAction, this, W));
	//Btn_W->setPosition(Vec2(origin.x + W->getWidth() * 2, origin.y + W->getHeight() * 3));
	Btn_W->setPosition(Vec2(origin.x + W->getContentSize().width * 2, origin.y + W->getContentSize().height * 3));
	Btn_W->setName("Btn_W");
	auto Btn_S = MenuItemLabel::create(S, CC_CALLBACK_0(HelloWorld::RunAction, this, S));
	//Btn_W->setPosition(Vec2(origin.x + W->getWidth() * 2, origin.y + W->getHeight() * 3));
	Btn_S->setPosition(Vec2(origin.x + W->getContentSize().width * 2, origin.y + W->getContentSize().height * 2));
	Btn_S->setName("Btn_S");
	auto Btn_A = MenuItemLabel::create(A, CC_CALLBACK_0(HelloWorld::RunAction, this, A));
	//Btn_W->setPosition(Vec2(origin.x + W->getWidth() * 2, origin.y + W->getHeight() * 3));
	Btn_A->setPosition(Vec2(origin.x + W->getContentSize().width, origin.y + W->getContentSize().height * 2));
	Btn_A->setName("Btn_A");
	auto Btn_D = MenuItemLabel::create(D, CC_CALLBACK_0(HelloWorld::RunAction, this, D));
	//Btn_W->setPosition(Vec2(origin.x + W->getWidth() * 2, origin.y + W->getHeight() * 3));
	Btn_D->setPosition(Vec2(origin.x + W->getContentSize().width * 3, origin.y + W->getContentSize().height * 2));
	Btn_D->setName("Btn_D");
	auto Btn_X = MenuItemLabel::create(X, CC_CALLBACK_0(HelloWorld::DieAction, this,X));
	//Btn_W->setPosition(Vec2(origin.x + W->getWidth() * 2, origin.y + W->getHeight() * 3));
	Btn_X->setPosition(Vec2(origin.x + visibleSize.width - W->getContentSize().width, origin.y + W->getContentSize().height * 3));
	Btn_X->setName("Btn_X");
	auto Btn_Y = MenuItemLabel::create(Y, CC_CALLBACK_0(HelloWorld::AttackAction, this, X));
	//Btn_W->setPosition(Vec2(origin.x + W->getWidth() * 2, origin.y + W->getHeight() * 3));
	Btn_Y->setPosition(Vec2(origin.x + visibleSize.width - W->getContentSize().width * 2, origin.y + W->getContentSize().height * 2));
	Btn_Y->setName("Btn_Y");

	auto menu = Menu::create(Btn_W, Btn_S, Btn_A, Btn_D, Btn_X, Btn_Y, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, 1);

    return true;
}
void HelloWorld::AttackAction(cocos2d::Label * label)
{
	if (player->numberOfRunningActions() != 0) return;
	auto animation = Animation::createWithSpriteFrames(attack, 0.1f);
	animation->setRestoreOriginalFrame(true);
	auto animate = Animate::create(animation);

	player->runAction(animate);
	if (pT->getPercentage() < 100)
	{
		pT->runAction(ProgressTo::create(2, pT->getPercentage() + 20));
	}
}
void HelloWorld::DieAction(cocos2d::Label * label)
{
	if (player->numberOfRunningActions() != 0) return;
	auto animation = Animation::createWithSpriteFrames(die, 0.1f);
	animation->setRestoreOriginalFrame(true);
	auto animate = Animate::create(animation);

	player->runAction(animate);
	if (pT->getPercentage() > 0)
	{
		pT->runAction(ProgressTo::create(2, pT->getPercentage() - 20));
	}
}
void HelloWorld::RunAction(cocos2d::Label* label) {
	float runtime=0.5;
	if (player->numberOfRunningActions() != 0) return;
	char op = label->getName().c_str()[0];
	CCLOG(label->getName().c_str());
	switch (op)
	{
	case 'W': {
		if (player->getPositionY() < visibleSize.height - 20)
		{
			player->runAction(MoveBy::create(runtime, Vec2(0, 20)));
		}
		break;
	}
	case 'S': {
		if (player->getPositionY() > 0 + 20)
		{
			player->runAction(MoveBy::create(runtime, Vec2(0, -20)));
		}
		break;
	}
	case 'A': {
		if (player->getPositionX() > 20)
		{
			player->runAction(MoveBy::create(runtime, Vec2(-20, 0)));
		}
		break;
	}
	case 'D': {
		if (player->getPositionX() < visibleSize.width - 20)
		{
			player->runAction(MoveBy::create(runtime, Vec2(20, 0)));
		}
		break;
	}
	default:
		break;
	}
	auto animation = Animation::createWithSpriteFrames(run, 0.1f);
	animation->setRestoreOriginalFrame(true);
	auto animate = Animate::create(animation);

	player->runAction(animate);
}
void HelloWorld::updateTime(float dt) {
	if (dtime > 0) {
		dtime--;
		time->setString(std::to_string(dtime));
	}
}

