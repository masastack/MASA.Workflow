(function () {

    const dblclick = new MouseEvent("dblclick", {
        view: window,
        bubbles: true,
        cancelable: false
    })

    function dblclickDrawflowNode(nodeId) {
        document.querySelector(`#node-${nodeId} .drawflow_content_node`).firstElementChild.firstElementChild.dispatchEvent(dblclick);
    }

    function setNodeIdToCustomElement(nodeId) {
        const customElement = document.querySelector(`#node-${nodeId} .drawflow_content_node`).firstElementChild;
        customElement.setAttribute("node-id", nodeId);
    }

    window._masaWorkflow = {
        dblclickDrawflowNode,
        setNodeIdToCustomElement
    }
})();