import React, { Component } from 'react';
import './custom.css';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <body>
        <section class="main-selection">
        {/* padding: 40px 0px;margin: 0px;height: 1000px; */}
          <form method="post" > {/*//style="width: 320px;height: 500px;padding: 10px;margin: 0px 350px;"*/}
            <div class="illustration">
              <strong>ATM</strong>
            </div>
            <div class="form-group">
              <div class="col"> {/*//style="height: 100px;border 3px solid var(--red);"*/}
                <div> {/*//style="padding:7px; height: 95px"
                  <span>Text</span> {/*style="padding: 5px 5px 5px 5px;margin: 0px;"*/}
                </div>
              </div>
              <div class="col"> {/*//style="height: 100px;border 3px solid var(--red);"*/}
                <div class="btn-group" role="group"> {/*//style="padding: 5px;height: 90px;;width: auto;width: 270px;"*/}
                  <button class="btn btn-primary" type="button">Withdraw</button> {/*//style="width: 100px;margin: 25px 0px 0px;"*/}
                  <button class="btn btn-primary" type="button">Balance</button> {/*////style="width: 100px;margin: 25px 0px 0px;"*/}
                </div>
              </div>
            </div>
          </form>
        </section>
      </body>
    );
  }
}
