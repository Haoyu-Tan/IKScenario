class Arm{
  
  Vec2 root;
  
  int numJoint;
  Vec2[] start_pos;
  float[] len;
  float[] angle;
  //float[] armWid;
  //float armWid;
  
  float[] maxAngle;
  float[] minAngle;
  
  float[] speedLimit;
  
  Arm(){
  }
  
  Arm(int numj, Vec2 startPos){
    numJoint = numj;
    root = startPos;

    
    start_pos = new Vec2[numJoint];
    len = new float[numJoint - 1];
    angle = new float[numJoint - 1];
    //armWid = new float[numJoint - 1];

    
    maxAngle = new float[numJoint - 1];
    minAngle = new float[numJoint - 1];
    
    speedLimit = new float[numJoint - 1];
    
    this.init();
  }
  
  void init(){
    //init pos
    start_pos[0] = root;
    
    //init len
    len[0] = 100;
    len[1] = 90;
    len[2] = 40;
    for (int i = 3; i < len.length; i++){
      len[i] = 40 - (i - 2)*5;
      if (len[i] <= 0) len[i] = 5;
    }
    
    //init angle
    for (int i = 0; i < angle.length; i++){
      angle[i] = 0.3;
    }
    
    /**
    //init armWid
    for (int i = 0; i < armWid.length; i++){
      armWid[i] = 20;
    }
    */
    
    //init maxAngle
    maxAngle[0] = PI/12;
    maxAngle[1] = 5*PI/6;
    maxAngle[2] = 5*PI/6;
    maxAngle[3] = PI/2;
    for (int i = 4; i < maxAngle.length; i++){
      maxAngle[i] = PI;
    }
    
    //init minAngle
    minAngle[0] = -PI/12;
    minAngle[1] = 0;
    minAngle[2] = 0;
    minAngle[3] = -PI/2;
    for (int i = 4; i < minAngle.length; i++){
      minAngle[i] = -PI;
    }
    
    for (int i = 0; i < speedLimit.length; i++){
      speedLimit[i] = 50*PI/180;
    }
  }
  
  void setMaxAngle(float[] max){
    this.maxAngle = max.clone();
    /**
    for (int i = 0; i < max.length; i++){
      this.maxAngle[i] = max[i];
    }
    */
  }
  
  void setMinAngle(float[] min){
    this.minAngle = min.clone();
    /**
    for (int i = 0; i < min.length; i++){
      this.minAngle[i] = min[i];
    }
    */
  }
  
  void fk(){
    
    start_pos[0] = root;
    
    float totalAngle = 0;
    for (int i = 1; i < start_pos.length; i++){
      //calculate angle until one before this joint
      //totalAngle += angle[i - 1];
      float currentAngle = 0;
      for(int j = 0; j < i; j ++){
        currentAngle += angle[j];
        
      }
      //get prev joint pos
      Vec2 prevPos = start_pos[i - 1];
      totalAngle = currentAngle;
      //calculate new coord
      float prevLen = len[i - 1];
      float new_x = cos(totalAngle) * prevLen;
      float new_y = sin(totalAngle) * prevLen;
      
      
      //check collision
      Vec2 newStart = prevPos;
      Vec2 newDir = new Vec2(new_x, new_y);
  
      //println("p3" + o1.pos.x + o1.pos.y);
      hitInfo hit = rayCircleListIntersect(obstacleCenter, obstacleRad, 3, newStart, newDir, 1);
      
     // hitInfo hit2 = rayCircleIntersect(shareRoot, rootRad + armWid/2, newStart, newDir, 1);
      
      if (hit.hit){
        
        Vec2 circleCenter = obstacleCenter[hit.index];
        float rad = obstacleRad[hit.index];
        
        /**
        numHIT++;
        println( numHIT + " circle " + hit.index + " radius: " + rad);
        */
        
        //calculate dir to circle
        Vec2 toCircle = circleCenter.minus(newStart);
        float deltaCC = acos(dot(newDir.normalized(), toCircle.normalized()));
        
        float beta = asin(rad / toCircle.length());
        
        float delta = deltaCC - beta;
        
        float sign = cross(newDir.normalized(), toCircle.normalized());
        if (sign < 0) delta *= -1;
        
        totalAngle -= angle[i-1];
        float finalAngle = angle[i-1] + delta;
        
        angle[i-1] = finalAngle;
        totalAngle += finalAngle;
        
        
        new_x = cos(totalAngle) * prevLen;
        new_y = sin(totalAngle) * prevLen;
        
        start_pos[i] = new Vec2(new_x, new_y).plus(prevPos);
      }
      
      else{
        //set new coord
        start_pos[i] = new Vec2(new_x, new_y).plus(prevPos);
      }
    }
    
  }
  
  void solve(Vec2 goal){
    
    //Vec2 goal = new Vec2(mouseX, mouseY);
    
    
    Vec2 startToGoal, startToEndEffector;
    float dotProd, angleDiff;
    
    
    
    //start from node before end point
    for(int i = numJoint - 2; i >= 0; i--){
      Vec2 endPoint = start_pos[numJoint - 1];
      Vec2 currJointPos = start_pos[i];
      
      startToGoal = goal.minus(currJointPos);
      startToEndEffector = endPoint.minus(currJointPos);
      
      dotProd = dot(startToGoal.normalized(), startToEndEffector.normalized());
      dotProd = clamp(dotProd, -1, 1);
      
      angleDiff = acos(dotProd);
      
      //clamp speed
      /**
      float maxSpeed = speedLimit[i] * deltaTime;
      if(angleDiff >= maxSpeed) angleDiff = maxSpeed;
      */
      if (isLimited)
        if (angleDiff > 1*PI/180) angleDiff = 1*PI/180;
      
      //direction
      if (cross(startToGoal.normalized(), startToEndEffector.normalized()) < 0){
        angle[i] += angleDiff;
      }
      else{
        angle[i] -= angleDiff;
      }
      
      //clamp angle
      if (isLimited){
        if (angle[i] < minAngle[i]) angle[i] = minAngle[i];
        if (angle[i] > maxAngle[i]) angle[i] = maxAngle[i];
      }
      
      this.fk();
    }
  }
  
  void render(){
    
    fill(255,201,107);
    
    float a = 0;
    for (int i = 0; i < numJoint - 1; i++){
      pushMatrix();
      translate(start_pos[i].x, start_pos[i].y);
      float currentAngle = 0;
      for (int j = 0; j <= i ;j ++){
        currentAngle += angle[j];
        
      }
      //a += angle[i];
      rotate(currentAngle);
      circle(0, 0, armWid);
      rect(0, -armWid/2, len[i], armWid);
      popMatrix();
    }
    
    pushMatrix();
    translate(start_pos[numJoint - 1].x, start_pos[numJoint - 1].y);
    //rotate(currentAngle);
    circle(0, 0,armWid);
    popMatrix();
    
  }
}
