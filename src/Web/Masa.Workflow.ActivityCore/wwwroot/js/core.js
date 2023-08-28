window.UpdateCustomElementValue = (el, value) => {
    console.log('e;', el, el.parentElement)


    // var element = document.querySelector(`tag[node-id="${nodeId}"]`)
    // console.log(tag, nodeId, value, 'get element =>', element)

    console.log('value', value)

    // el.parentElement.value = JSON.stringify({
    //     data: value
    // })
    // https://github.com/jerosoler/Drawflow/issues/490
    console.log('el.parentElement.data', el.parentElement.data)
    
    el.parentElement.value = "aaaaaaaaaabcccccccccccc";
}