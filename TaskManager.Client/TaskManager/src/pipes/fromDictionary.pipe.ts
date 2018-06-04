import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'fromDictionary'
})
export class FromDictionaryPipe implements PipeTransform {
    transform(value: number, dictionary: any[]): string {

        var filtered = dictionary.filter(function (item) {
            return (item.ID !== undefined && item.ID === value) || (item.Id !== undefined && item.Id === value) || (item.id !== undefined && item.id === value);
        });

        return filtered && filtered[0] ? ((filtered[0].Name || filtered[0].name)) : null;
    }
}