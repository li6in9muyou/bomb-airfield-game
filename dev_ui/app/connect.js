let tool = {
   createRoom(){
     //告诉后端本机已经创建房间,其他人输入IP地址可以加入
   },
   tellCreateStart(){
    return true //告诉我是否有人加入房间
   },
   tellJoinStart(ip){
    return true //传入要加入房间的ip 告诉我是否加入房间成功
   },
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
   }
}
export default tool