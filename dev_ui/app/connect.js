/*
 * @Author: error: git config user.name && git config user.email & please set dead value or install git
 * @Date: 2022-11-01 22:09:56
 * @LastEditors: error: git config user.name && git config user.email & please set dead value or install git
 * @LastEditTime: 2022-11-07 22:07:45
 * @FilePath: \炸飞机\app\connect.js
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
let tool = {
   getMyIP: function getMyIP(){
    return 123
   },//获取本机的后端IP 用于构建房间
   getYourIp: function getYourIp(){
   return 123
   },
   getYourAttack: function getYourAttack(){
      return 123
   },
   sendMyAttack: function sendMyAttack(){
      return true
   },
   sendMyPosition: function sendMyPosition(){
      return true
   },
   allReady(){
      return true
   },
   myResult(){
      return true
   },
   isFail(){
      return 123
   },
   sendFail(){
      return 123
   }
}
export default tool