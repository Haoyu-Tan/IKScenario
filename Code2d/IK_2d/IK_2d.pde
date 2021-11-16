
/**
Name: Haoyu Tan
ID#: 5677259
*/


//ArrayList<Arm> arms;
Arm armLeft;
Arm armRight;

float armWid = 20;

//init obstacle
Vec2[] obstacleCenter = {new Vec2(300, 450), new Vec2(500, 500), new Vec2(100,500)};
float[] obstacleRad = {(50.0 + armWid/2), (80+ armWid/2), (30 + armWid/2)};

int numHIT = 0;

boolean isLimited;

Vec2 shareRoot;
float rootRad;
Vec2 goal;
float goalSpeed = 100.0;

boolean leftPressed;
boolean rightPressed;
boolean upPressed;
boolean downPressed;


void setup(){
  size(720,560);
  surface.setTitle("Project3 2D");
  
  init();
}

void init(){
  isLimited = true;
  
  shareRoot = new Vec2(350, 200);
  rootRad = 65;
  
  goal = new Vec2(350, 350);
  
  //init arm
  armLeft = new Arm(5, new Vec2(420,200));
  armRight = new Arm(5, new Vec2(280, 200)); 
  
  float[] max = {13*PI/12, 0, 0, PI/2};
  float[] min = {11*PI/12, -5*PI/6, -5*PI/6, -PI/2};
  armRight.setMaxAngle(max);
  armRight.setMinAngle(min);
  
  leftPressed = false;
  rightPressed = false;
  downPressed = false;
  upPressed = false;
  
}

void draw(){
  
 
  background(250,250,250);
  
  updateGoal(1.0/frameRate);
  
  
  //update left hand
  armLeft.fk();
  armLeft.solve(goal);
  armLeft.render();
  
  //update right hand
  armRight.fk();
  armRight.solve(goal);
  armRight.render();
  
  drawGoal();
  
  drawObstacles();
  
  drawRoot();
  
  
  
}

void updateGoal(float dt){
  Vec2 goalVel = new Vec2(0, 0);
  if (leftPressed) goalVel.add(new Vec2(-1, 0));
  if (rightPressed) goalVel.add(new Vec2(1, 0));
  if (upPressed) goalVel.add(new Vec2(0, -1));
  if (downPressed) goalVel.add(new Vec2(0, 1));
  
  if (goalVel.x != 0 || goalVel.y != 0) {
    goalVel.normalize();
  }
    
  goalVel.mul(goalSpeed);
  
  goal.add(goalVel.times(dt));
  
}

void keyPressed(){
  if (key == 'z'){
    isLimited = !isLimited;
  }
  
  if (key == 'w') upPressed = true;
  if (key == 's') downPressed = true;
  if (key == 'a') leftPressed = true;
  if (key == 'd') rightPressed = true;
  
}

void keyReleased(){
  if (key == 'w') upPressed = false;
  if (key == 's') downPressed = false;
  if (key == 'a') leftPressed = false;
  if (key == 'd') rightPressed = false;
}

float obstacleSpeed = 0.5f;
float prevMouseX;
float prevMouseY;

void mousePressed(){
  prevMouseX = mouseX;
  prevMouseY = mouseY;
}

void mouseDragged(){
  for (int i = 0; i < obstacleCenter.length; i++){
    if (pointInCircle(obstacleCenter[i], obstacleRad[i], new Vec2(mouseX, mouseY), 0)){
     
      obstacleCenter[i].x = obstacleCenter[i].x + (mouseX - prevMouseX) * obstacleSpeed;
      obstacleCenter[i].y = obstacleCenter[i].y +  (mouseY - prevMouseY) * obstacleSpeed;
      break;
    }
  }
  prevMouseX = mouseX;
  prevMouseY = mouseY;
}


void drawGoal(){
  fill(0,255,0);
  pushMatrix();
  translate(goal.x, goal.y);
    
  circle(0, 0, 50);
  
  popMatrix();
}

void drawRoot(){
  fill(255,201,107);
  pushMatrix();
  translate(shareRoot.x, shareRoot.y);
    //println("in draw, dia: " + (rad-armWid/2) * 2);
  circle(0, 0, rootRad*2);
  //rect(armWid/2, -armWid/2, 200, armWid*6);
  popMatrix();
}

void drawObstacles(){
  fill(255, 0, 0);
  for (int i = 0; i < obstacleCenter.length; i++){
    Vec2 oPos = obstacleCenter[i];
    float rad = obstacleRad[i];
    pushMatrix();
    translate(oPos.x, oPos.y);
    //println("in draw, dia: " + (rad-armWid/2) * 2);
    circle(0, 0, (rad - armWid/2)*2);
    popMatrix();
  }
}
