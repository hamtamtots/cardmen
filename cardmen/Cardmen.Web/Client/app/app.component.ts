import { Component } from '@angular/core';

import { ArticleService } from './article.service'

@Component({
    selector: 'cardmen-app',
    templateUrl: 'app.component.html',
    providers: [ArticleService]
})
export class AppComponent {

    private articleService: ArticleService;

    constructor(articleService: ArticleService) {
        this.articleService = articleService;
    }

    createArticle() {
        this.articleService.createArticle();
    }
}