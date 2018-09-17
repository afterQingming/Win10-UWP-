#include "HelloWorldScene.h"
#include "SimpleAudioEngine.h"

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
//
//  MainScene.cpp
//  helloworld
//
//  Created by apple on 16/9/19.
//
//

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
    //////////////////////////////
    // 1. super init first
    if ( !Scene::init() )
    {
        return false;
    }

    auto visibleSize = Director::getInstance()->getVisibleSize();
    Vec2 origin = Director::getInstance()->getVisibleOrigin();
	


	auto sprite = Sprite::create("atom.jpg");
	sprite->setPosition(50, 50);
	sprite->setAnchorPoint(Vec2(0.5, 0.5));
	this->addChild(sprite);// 添加到层
	auto act= ScaleTo::create(0, 0.2);
	sprite->runAction(act);


						   //声明
	auto listener1 = EventListenerTouchOneByOne::create();
	listener1->setSwallowTouches(true);

	//通过 lambda 表达式 直接实现触摸事件的回掉方法
	listener1->onTouchBegan = [=](Touch* touch, Event* event) {
		auto target = static_cast<Sprite*>(event->getCurrentTarget());

		Point locationInNode = target->convertToNodeSpace(touch->getLocation());
		Size s = target->getContentSize();
		Rect rect = Rect(0, 0, s.width, s.height);

		if (rect.containsPoint(locationInNode))
		{
			log("sprite began... x = %f, y = %f", locationInNode.x, locationInNode.y);
			target->setPosition(((double)(rand() % 10)) / 10 *visibleSize.width+origin.x,
				((double)(rand() % 10)) / 10 *visibleSize.height+origin.y);
			return true;
		}
		return false;
	};
	//将触摸事件绑定到精灵身上
	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener1, sprite);

    /////////////////////////////
    // 2. add a menu item with "X" image, which is clicked to quit the program
    //    you may modify it.

    // add a "close" icon to exit the progress. it's an autorelease object
    auto closeItem = MenuItemImage::create(
                                           "CloseNormal.png",
                                           "CloseSelected.png",
                                           CC_CALLBACK_1(HelloWorld::menuCloseCallback, this));

    if (closeItem == nullptr ||
        closeItem->getContentSize().width <= 0 ||
        closeItem->getContentSize().height <= 0)
    {
        problemLoading("'CloseNormal.png' and 'CloseSelected.png'");
    }
    else
    {
        float x = origin.x + visibleSize.width - closeItem->getContentSize().width/2;
        float y = origin.y + closeItem->getContentSize().height/2;
        closeItem->setPosition(Vec2(x,y));
    }

    // create menu, it's an autorelease object
    auto menu = Menu::create(closeItem, NULL);
    menu->setPosition(Vec2::ZERO);
    this->addChild(menu, 1);

    /////////////////////////////
    // 3. add your codes below...

 
	CCDictionary* message = CCDictionary::createWithContentsOfFile("tar.xml");    //读取xml文件，文件在Resources目录下
	auto name = message->valueForKey("name"); 
	auto num = message->valueForKey("num"); 
	log((name->_string + '\n' + num->_string).c_str());
	Label* label = Label::createWithSystemFont(name->_string+'\n'+num->_string, "Arial", 17);
	//auto label = Label::createWithTTF(b, "fonts/arial.ttf", 24);
	if (label == nullptr)
	{
		problemLoading("'fonts/arial.ttf'");
	}
	else
	{
		// position the label on the center of the screen
		label->setPosition(Vec2(origin.x + visibleSize.width / 2,
			origin.y + visibleSize.height - label->getContentSize().height));

		// add the label as a child to this layer
		this->addChild(label, 1);
	}
	

    // add "HelloWorld" splash screen"
    
    return true;
}


void HelloWorld::menuCloseCallback(Ref* pSender)
{
    //Close the cocos2d-x game scene and quit the application
    Director::getInstance()->end();

    #if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
    exit(0);
#endif

    /*To navigate back to native iOS screen(if present) without quitting the application  ,do not use Director::getInstance()->end() and exit(0) as given above,instead trigger a custom event created in RootViewController.mm as below*/

    //EventCustom customEndEvent("game_scene_close_event");
    //_eventDispatcher->dispatchEvent(&customEndEvent);


}
