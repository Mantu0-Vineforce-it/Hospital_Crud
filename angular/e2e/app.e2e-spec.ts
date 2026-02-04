import { UserCrudTemplatePage } from './app.po';

describe('UserCrud App', function () {
    let page: UserCrudTemplatePage;

    beforeEach(() => {
        page = new UserCrudTemplatePage();
    });

    it('should display message saying app works', () => {
        page.navigateTo();
        expect(page.getParagraphText()).toEqual('app works!');
    });
});
