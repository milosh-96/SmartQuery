(function(){
    let template = document.createElement("template");
    template.innerHTML = ` 
    <div style="display:flex">
    <div id="result"></div>
    <button id="check-button">Check</button>
    </div>
    `;
    let inputQuery = "";
    let onCheckEventName = "FillInChecked";
    class FillIn extends HTMLElement {
        constructor() {
            super();
            this.attachShadow({ mode: 'open' });
            this.shadowRoot.appendChild(template.content.cloneNode(true));
        }
        connectedCallback() {
            inputQuery = this.getAttribute("query");
            this.shadowRoot.getElementById("check-button").addEventListener("click",()=>{this.check()});
        }
        async checkInApiIfQueryExist() {
            let result = false;
           await fetch('/Entries/Api/Single?Name='+inputQuery,{method:'HEAD'})
                .then((response)=>{
                    if(response.status == 404) {
                        result = false;
                    }
                    else {
                        result = true;
                    }
                })
               
                .catch((error)=>{

            });
            let customEvent = new CustomEvent(onCheckEventName,{detail:result,composed:true});
            document.dispatchEvent(customEvent);
            return result;
        }
        async check(){
            inputQuery = this.getAttribute("query");

            if(inputQuery.length > 1) {
                this.shadowRoot.getElementById("result").innerHTML = "please wait...";

               let queryExists = await this.checkInApiIfQueryExist();
                if(queryExists) {
                    this.shadowRoot.getElementById("result").innerHTML = "This item already exists!";
                }
                else {
                    this.shadowRoot.getElementById("result").innerHTML = "Good! This item doesn't exist!";

                }
            }
           
        }
    }
    window.customElements.define("fill-in",FillIn);
})();