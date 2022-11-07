/*
 * @Author: error: git config user.name && git config user.email & please set dead value or install git
 * @Date: 2022-10-31 15:14:06
 * @LastEditors: error: git config user.name && git config user.email & please set dead value or install git
 * @LastEditTime: 2022-11-01 22:57:50
 * @FilePath: \炸飞机\app\main.js
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
import tool from './connect.js'//函数工具类
const ensure = document.querySelector('.ensure')
const backLast = document.querySelector('.backLast')
const chooseGoal =  document.querySelector('.chooseGoal')
const joinRoom = document.querySelector('.joinRoom')
const content = document.querySelector('.content')
const button =  document.querySelector('.buttonFunction')
const createRoom = document.querySelector('.createRoom')
const IPInput = document.querySelector('.IPInput')
const tip =  document.querySelector('.tip')
let type
function buttonHandle(){
  button.style.display = 'flex'
  ensure.style.display = 'block'
  backLast.style.display = 'block'
}
function backButton(){
  button.style.display = 'none'
  ensure.style.display = 'none'
  backLast.style.display = 'none'
  content.style.display= 'flex'
  IPInput.style.display = 'none'
  tip.style.display = 'none'
}
function chooseWay(){
  chooseGoal.style.top= '0'
  joinRoom.addEventListener('click',function(){
  content.style.display = 'none'
  IPInput.style.display = 'block'
  buttonHandle()
  type = 'join'
  },false)
  ensure.addEventListener('click',function(e){
    if(type === 'join'){
    let ip = IPInput.value.trim()
    if(!ip){
      tip.style.display = 'block'
      tip.innerText = 'IP不能为空'
    }else{
    document.querySelector('.chooseGoal').style.top= '-2000px'
    }
  }else{
    document.querySelector('.chooseGoal').style.top= '-2000px'
  }
  },false)
 createRoom.addEventListener('click',function(){
    tip.style.display = 'block'
    content.style.display = 'none'
    tip.innerText =  '本机的IP地址'+ tool.getIP()+ '\n' + '等待其它人加入房间'
    buttonHandle()
  })
  backLast.addEventListener('click',backButton)
}
chooseWay()