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

    async function downloadFileFromStream(fileName, contentStreamReference) {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        console.log('fileName', fileName)
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    }

    window._masaWorkflow = {
        dblclickDrawflowNode,
        setNodeIdToCustomElement,
        downloadFileFromStream
    }
})();