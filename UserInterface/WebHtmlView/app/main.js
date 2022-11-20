import Connector from "./connect.js";
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
let type = 'create',time = 0;
let tool=new Connector();
console.log(tool)
tool.connect(description)
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
 createRoom.addEventListener('click',function(){
    tip.style.display = 'block'
    content.style.display = 'none'
    tip.innerText =  '创建房间成功 其他人输入您的IP地址即可加入房间'
    buttonHandle()
  })
  ensure.addEventListener('click',function(e){
    if(type === 'join'){
    let ip = IPInput.value.trim()
    if(!ip){
      tip.style.display = 'block'
      tip.innerText = 'IP不能为空'
    }else{
    document.querySelector('.chooseGoal').style.top= '-2000px'//房间选择功能已经完成
    joinRoomHandle(ip)
    gameService()
    }
  }else{
    document.querySelector('.chooseGoal').style.top= '-2000px'//房间选择功能已经完成
    createRoomHandle()
    gameService()
  }
  },false)
  backLast.addEventListener('click',backButton)//返回上一级的处理函数
}
chooseWay()
function createRoomHandle(){
 tool.sendMyIP('')
}
function joinRoomHandle(ip){
 tool.sendMyIP(ip)
}
//此函数接管游戏过程中的所有服务 
function gameService(){
    console.log(type)
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
function handleHead(arr){
    for(let i = 0 ; i<arr.length;i++){
        if(arr[i][2] === 1 || arr[i][2] === 3){
            arr[i][0] = (arr[i][0] - 2) * 84 + 32
        }
        if(arr[i][2] === 2){
            arr[i][0] = (arr[i][0] - 3) * 84 
        }
        if(arr[i][2] === 4){
            arr[i][0] = arr[i][0]* 84
        }
        if(arr[i][2] === 2 || arr[i][2] === 4){
            arr[i][1] = (arr[i][1] - 2) * 84
        }
        if(arr[i][2] === 1){
            arr[i][1] = arr[i][1] * 84
        }
        if(arr[i][2] === 3){
            arr[i][1] = (arr[i][1] - 3) * 84
        }
    }
    return arr;
}
function imgPlane(positionArr){
    positionArr = handleHead((positionArr))
  rotateHandle(myOne,positionArr[0][2])
  myOne.style.left = positionArr[0][0] + 'px'
  myOne.style.top = positionArr[0][1] -25 +'px';
  rotateHandle(myTwo,positionArr[1][2])
    myTwo.style.left = positionArr[1][0] + 'px';
    myTwo.style.top = positionArr[1][1] -25+ 'px';
    rotateHandle(myThree,positionArr[2][2])
  myThree.style.left = positionArr[2][0] + 'px';
  myThree.style.top = positionArr[2][1] -25+ 'px'
  description.innerText = '游戏正式开始!'
  gameStart()
}

function doAttack(xy){
    xy = xy.map(function(item){
        return Number(item)
    })
    tool.sendMyAttack(xy)//返回的轰炸结果
    let num = xy[1] * 10 + xy[0]
    yourLi[num].innerHTML = "<img src='../img/wound.png' width='84' height='84'></p>"
    description.innerText = '等待对手轰炸'
    if(type !== 'create' || time !== 0) {
        let res = tool.getYourAttack()
        for (let i = 0; i < res.length; i++) {
            if (res[i]['x'] >= 10) {
                num = res[i]['y'] * 10 + res[i]['x'] - 10
                let key = wound(res[i]['result'])
                yourLi[num].innerHTML=`<img src='../img/wound.png' width='84' height='84'><p class="key">${key}</p>`
            } else {
                num = res[i]['y'] * 10 + res[i]['x']
                let key = wound(res[i]['result'])
                myLi[num].innerHTML = `<img src='../img/wound.png' width='84' height='84'><p class="key">${key}</p>`
            }
        }
    }
    time++;
}

window.doAttack = doAttack

function gameStart(){
   attackEnsure.addEventListener('click',function(e){
      let target = attack.value.split(',');
      doAttack(target);
   },false)
   AI.addEventListener('click',function(){
      description.innerText = '正在进行AI托管'
      AIGame()
   },false)
}
function wound(value){
   if(value === 'Miss'){
    return '空'
   }
   if(value === 'Hit'){
    return '伤'
   }
   if(value === 'Destroyed'){
    return '毁'
   }
}
function AIGame(){

}