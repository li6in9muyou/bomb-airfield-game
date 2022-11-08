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
const description = document.querySelector('.description')
const placePlane = document.querySelector('.placePlane')
const submitPlane = document.querySelector('.submitPlane')
const myPlane = document.querySelectorAll('.myPlane')
const planeTip = document.querySelector('.planeTip')
const myOne =  document.querySelector('#my-one')
const myTwo =  document.querySelector('#my-two')
const myThree =  document.querySelector('#my-three')
const attackEnsure = document.querySelector('.attackEnsure')
const attack = document.querySelector('.attack')
const yourLi = document.querySelectorAll('.enemy ul li')
const myLi = document.querySelectorAll('.myself ul li')
const failure = document.querySelector('.failure')
const AI = document.querySelector('.AI')
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
  type = ''
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
    document.querySelector('.chooseGoal').style.top= '-2000px'//房间选择功能已经完成
    if(joinRoomHandle()){
      gameService()
    }
    }
  }else{
    document.querySelector('.chooseGoal').style.top= '-2000px'//房间选择功能已经完成
    if(createRoomHandle()){
      gameService()
    }
  }
  },false)
 createRoom.addEventListener('click',function(){
    tip.style.display = 'block'
    content.style.display = 'none'
    tip.innerText =  '本机的IP地址'+ tool.getMyIP()+ '\n' + '等待其它人加入房间'
    buttonHandle()
  })
  backLast.addEventListener('click',backButton)
}
chooseWay()
function createRoomHandle(){
  if(tool.getYourIp()){
    return true
  }else{
    return createRoomHandle()
  }
}
function joinRoomHandle(){
  if(tool.getYourIp()){
    return true
  }else{  
    return createRoomHandle()
  }
}
//此函数接管游戏过程中的所有服务 
function gameService(){
  description.innerHTML = '游戏开始咯!'
  setTimeout(function(){
     description.innerHTML = '请你进行飞机布局'
     placePlane.style.top = '0'
  },2000)
  submitPlane.addEventListener('click',function(e){
    let positionArr = []
    for(let i = 0; i < myPlane.length; i++){
    let item = myPlane[i].value.split(',')
    item = item.map(function(item){
     return Number(item)
    })
     positionArr.push(item)
    }
    if(!tool.sendMyPosition(positionArr)){
      planeTip.innerText ='飞机位置冲突,请你重新布置'
    }else{
      placePlane.style.top = '-2000px'
      imgPlane(positionArr)
    }
  },false)
}
function rotateHandle(goal,deg){
  if(deg === 1){
    goal.style.transform = 'rotate(270deg)'
   }
  if(deg === 2){
    goal.style.transform = 'rotate(0deg)'
   }
   if(deg === 3){
    goal.style.transform = 'rotate(90deg)'
   }
   if(deg === 4){
    goal.style.transform = 'rotate(180deg)'
   }
}
function imgPlane(positionArr){
  myOne.style.left = (positionArr[0][0] - 3) * 84 + 'px'
  myOne.style.top = (positionArr[0][1] -2) * 84 + 'px'
  rotateHandle(myOne,positionArr[0][2])
  myTwo.style.left = (positionArr[1][0] - 3) * 84 + 'px'
  myTwo.style.top = (positionArr[1][1] - 2) * 84 + 'px'
  rotateHandle(myTwo,positionArr[1][2])
  myThree.style.left = (positionArr[2][0] - 3) * 84 + 'px'
  myThree.style.top = (positionArr[2][1] - 2) * 84 + 'px'
  rotateHandle(myThree,positionArr[2][2])
  if(tool.allReady){
    description.innerText = '游戏正式开始!'
    gameStart()
  }
}
function gameStart(){
   attackEnsure.addEventListener('click',function(e){
      let target = attack.value.split(',');
      target = target.map(function(item){
        return Number(item)
      })
      let res = tool.sendMyAttack(target)//返回的轰炸结果
      let num = res[1] * 10 + res[0] 
      if(res){
       yourLi[num].style.background = 'url(../img/wound.png)'
       yourLi[num].innerText = wound(res[2])
       description.innerTeres = '等待对手轰炸'
       res = yourAttack()
       num = res[1] * 10 + res[0] 
       myLi[num].style.background = 'url(../img/wound.png)'
       if(tool.myResult){
       description.innerText = '请你进行轰炸'
       }else{
       description.innerText = '您已经失败! 对手获得胜利'
       }
      }
   },false)
   failure.addEventListener('click',function(e){
    if(sendFail()){
      description.innerText = '投降成功! 游戏结束'
    }
   })
   AI.addEventListener('click',function(){
      description.innerText = '正在进行AI托管'
      AIGame()
   },fale)
}
function wound(value){
   if(value === 0){
    return '空'
   }
   if(value === 1){
    return '伤'
   }
   if(value === 2){
    return '死'
   }
}
function yourAttack(){
  let res = tool.getYourAttack()
  if(!res){
    return res = yourAttack()
  }else{
    return res
  }
}
function AIGame(){
z
}