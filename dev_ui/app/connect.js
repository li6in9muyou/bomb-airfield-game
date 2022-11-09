let tool = {
   getYourAttack: function getYourAttack(){
      return 123//得到敌方的攻击位置和结果
   },
   sendMyAttack: function sendMyAttack(){
      return true//发送我的攻击位置
   },
   sendMyPosition: function sendMyPosition(){
      return true//发送飞机布局位置
   },
   getYourReady(){
      return true//获取对方是否摆放飞机成功
   },
   sendMyReady(){
    //告诉后端已经摆放成功
   },
   myResult(des){
    //des是提示框元素对象 提示信息直接给它的innerText赋值就行
   },
   sendMyIP(ip){
   //创建房间ip为空字符串 加入房间为ip地址
   }
}
export default tool