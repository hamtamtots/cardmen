import { Injectable } from '@angular/core';

@Injectable()
export class ArticleService {

    constructor() {
        $.connection.articleHub.client.notifyArticleOperationSuccessful = (articleId) => {
            console.log("article op successful, " + articleId);
        }
        $.connection.hub.start().done(() => {
            console.log("SigR connected");
        })
    }

    createArticle() {
        $.connection.articleHub.server.createArticle();
        // this needs to be made async, callback/promise or something
    }
}